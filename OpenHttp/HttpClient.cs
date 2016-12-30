using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using OpenHttp.Enum;
using OpenHttp.Expand;

namespace OpenHttp
{
    /// <summary>
    /// HttpClient
    /// 不同实例之间Cookie不互通
    /// </summary>
    public class HttpClient
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public HttpClient()
        {
            ServicePointManager.MaxServicePoints = 0;
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = true;
            //linux下无法使用
            //ServicePointManager.EnableDnsRoundRobin = true;
            ServicePointManager.MaxServicePointIdleTime = int.MaxValue;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Cookie对象
        /// </summary>
        public CookieContainer Cookie { get; set; } = new CookieContainer();
        #endregion

        #region byte[] Load(Httphead head)
        /// <summary>
        /// 获取Text资源,如Html,Js,Css
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public byte[] Load(ref HttpHead head)
        {
            HttpWebRequest myRequest = null;
            HttpWebResponse myResponse = null;
            Stream stream = null;
            MemoryStream memoryStream = null;
            try
            {
                myRequest = GetRequest(head);
                myResponse = GetResponse(myRequest, ref head);
                stream = myResponse?.GetResponseStream();
                memoryStream = new MemoryStream();
                stream?.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            finally
            {
                memoryStream?.Close();
                memoryStream?.Dispose();
                stream?.Close();
                stream?.Dispose();
                myResponse?.Close();
                myResponse?.Dispose();
                myRequest?.Abort();
            }
        }
        #endregion

        #region HttpWebRequest GetRequest(Httphead head)
        /// <summary>
        /// 获取HttpWebRequest
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest GetRequest(HttpHead head)
        {
            var myRequest = (HttpWebRequest)WebRequest.Create(UrlPack(head));
            //myRequest.DefaultCachePolicy
            //myRequest.DefaultMaximumErrorResponseLength
            //myRequest.DefaultMaximumResponseHeadersLength
            myRequest.Accept = head.Accept;
            //myRequest.Address
            myRequest.AllowAutoRedirect = head.AllowAutoRedirect;
            //myRequest.AllowReadStreamBuffering
            //myRequest.AllowWriteStreamBuffering
            myRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;
            //myRequest.ClientCertificates
            //myRequest.Connection
            //myRequest.ConnectionGroupName
            //myRequest.ContentType
            //myRequest.ContentLength
            //myRequest.ContinueDelegate
            //myRequest.ContinueTimeout
            if (head.CookieState) myRequest.CookieContainer = Cookie;
            //myRequest.Credentials
            //myRequest.Date
            //myRequest.Expect
            //myRequest.HaveResponse
            myRequest.Headers.Add(head.RequestHeaders);
            if (!string.IsNullOrWhiteSpace(head.Host)) myRequest.Host = head.Host;
            //myRequest.IfModifiedSince
            myRequest.KeepAlive = head.KeepAlive;
            myRequest.MaximumAutomaticRedirections = head.MaximumAutomaticRedirections;
            //myRequest.MaximumResponseHeadersLength
            //myRequest.MediaType = "text/html; charset=utf8";
            myRequest.Method = head.Method.ToString();
            //myRequest.Pipelined
            //myRequest.PreAuthenticate
            myRequest.ProtocolVersion = head.Version;
            if (null != head.Proxy) myRequest.Proxy = head.Proxy;
            //myRequest.ReadWriteTimeout
            if (!string.IsNullOrWhiteSpace(head.Referer)) myRequest.Referer = head.Referer;
            //myRequest.RequestUri
            //myRequest.SendChunked
            //myRequest.ServerCertificateValidationCallback
            myRequest.ServicePoint.BindIPEndPointDelegate = head.BindIpEndPointDelegate;
            //myRequest.ServicePoint.ConnectionLimit = int.MaxValue;
            //myRequest.SupportsCookieContainer
            //myRequest.Timeout
            //myRequest.TransferEncoding
            //myRequest.UnsafeAuthenticatedConnectionSharing
            //myRequest.UseDefaultCredentials
            myRequest.UserAgent = head.UserAgent;

            return head.Method == Method.Get ? myRequest : PostPack(myRequest, head);
        }
        #endregion

        #region string UrlPack(HttpHead head)
        private static string UrlPack(HttpHead head)
        {
            switch (head.Method)
            {
                case Method.Get:
                    var keyValueList = from key in head.Data.AllKeys
                                              let keyValues = head.Data.GetValues(key)
                                              where keyValues != null
                                              from value in keyValues
                                              select $"{key}={value}";
                    var getData = string.Join("&", keyValueList);

                    return string.IsNullOrWhiteSpace(getData) ? head.Url : string.Join(head.Url.IndexOf("?", StringComparison.Ordinal) > 0 ? "&" : "?", head.Url, getData);
                case Method.Post:
                    return head.Url;
                default:
                    return head.Url;
            }
        }
        #endregion

        #region MyRegion HttpWebRequest PostPack(HttpWebRequest myRequest, HttpHead head)
        private static HttpWebRequest PostPack(HttpWebRequest myRequest, HttpHead head)
        {
            Stream stream = null;
            try
            {
                myRequest.ContentType = head.ContentType.ToDescriptionName();
                var postBytes = new List<byte[]>();
                #region 组装GetRequestStream
                switch (head.ContentType)
                {
                    case ContentType.Default:
                        var keyValueList = from key in head.Data.AllKeys
                                           let keyValues = head.Data.GetValues(key)
                                           where keyValues != null
                                           from value in keyValues
                                           select $"{key}={value}";

                        var defaultData = string.Join("&", keyValueList);
                        var defaultBytes = Encoding.UTF8.GetBytes(defaultData);

                        postBytes.Add(defaultBytes);
                        break; ;
                    case ContentType.Json:
                        var jsonData = head.Data.AllKeys.ToDictionary(c => c, c => head.Data.GetValues(c)).ToJson();
                        var jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                        postBytes.Add(jsonBytes);
                        break; ;
                    case ContentType.Xml:
                        var xmlData = head.Data.AllKeys.ToDictionary(c => c, c => head.Data.GetValues(c)).ToXml();
                        var xmlBytes = Encoding.UTF8.GetBytes(xmlData);

                        postBytes.Add(xmlBytes);
                        break; ;
                    case ContentType.Stream:
                        #region 组装PostBytes
                        var boundary = DateTime.Now.Ticks.ToString("x");
                        myRequest.ContentType = string.Format(myRequest.ContentType, boundary);
                        var boundarybytes = Encoding.UTF8.GetBytes($"\r\n------------{boundary}\r\n");
                        var endbytes = Encoding.UTF8.GetBytes($"\r\n------------{boundary}--\r\n");

                        foreach (var data in head.Data.AllKeys.ToDictionary(c => c, c => head.Data.GetValues(c)))
                        {
                            foreach (var value in data.Value)
                            {
                                postBytes.Add(boundarybytes);
                                postBytes.Add(Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"{data.Key}\"\r\nContent-Type: text/plain\r\n\r\n{value}"));
                            }
                        }

                        foreach (var file in head.Files)
                        {
                            postBytes.Add(boundarybytes);
                            postBytes.Add(Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"{file.Item1}\"; filename=\"{file.Item2}\"\r\nContent-Type: application/octet-stream\r\n\r\n"));
                            postBytes.Add(file.Item3);
                        }

                        postBytes.Add(endbytes);
                        #endregion

                        break;
                }
                #endregion
                myRequest.ContentLength = postBytes.Sum(c => c.Length);
                stream = myRequest.GetRequestStream();
                foreach (var postByte in postBytes) stream.Write(postByte, 0, postByte.Length);
                return myRequest;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }


        #endregion

        #region byte[] GetResponseBytes(HttpWebRequest myRequest, ref HttpHead head)

        private HttpWebResponse GetResponse(HttpWebRequest myRequest, ref HttpHead head)
        {
            HttpWebResponse myResponse;

            try
            {
                myResponse = (HttpWebResponse)myRequest.GetResponse();
            }
            catch (WebException ex)
            {
                myResponse = (HttpWebResponse)ex.Response;
            }

            if (null == myResponse) return null;

            head.Encod = head.Encod ?? Encoding.GetEncoding(string.IsNullOrEmpty(myResponse.CharacterSet) ? "utf-8" : myResponse.CharacterSet);
            //myResponse.ContentEncoding
            //myResponse.ContentLength
            //myResponse.ContentType
            if (head.CookieState) Cookie.Add(myResponse.Cookies);
            head.ResponseHeaders = myResponse.Headers;
            //myResponse.IsMutuallyAuthenticated
            //myResponse.LastModified
            //myResponse.Method
            //myResponse.ProtocolVersion
            //myResponse.ResponseUri
            //myResponse.Server
            //myResponse.StatusCode
            //myResponse.StatusDescription
            //myResponse.SupportsHeaders
            return myResponse;
        }
        #endregion
    }
}
