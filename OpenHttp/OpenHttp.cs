using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenHttp
{
    /// <summary>
    /// Http头信息
    /// </summary>
    public class HttpHead
    {

        /// <summary>
        /// 初始化HttpHread
        /// </summary>
        public static HttpHead Builder => new HttpHead();

        #region 请求头

        /// <summary>
        /// 请求的Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 希望接受的数据类型,默认text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
        /// </summary>
        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

        /// <summary>
        /// 自动重定向,默认开启
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary>
        /// 请求的参数集合
        /// </summary>
        public Object Data { get; private set; } = new NameValueCollection();

        /// <summary>
        /// 上传的文件
        /// </summary>
        public List<Tuple<string, string, byte[]>> Files { get; private set; } = new List<Tuple<string, string, byte[]>>();

        /// <summary>
        /// Post数据方式
        /// </summary>
        public ContentType ContentType { get; private set; } = ContentType.Default;

        /// <summary>
        /// 当前Head是否使用Cookie,默认开启
        /// </summary>
        public bool CookieState { get; set; } = true;

        /// <summary>
        /// 请求头
        /// </summary>
        public WebHeaderCollection RequestHeaders { get; } = new WebHeaderCollection { { "Accept-Encoding", "gzip,deflate" }, { "Accept-Language", "zh-CN,zh;q=0.8" } };

        /// <summary>
        /// 请求的Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 是否建立持久性连接,true
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// 自动重定向的次数,默认50
        /// </summary>
        public int MaximumAutomaticRedirections { get; set; } = 50;

        /// <summary>
        /// 请求方式,默认Get
        /// </summary>
        public Method Method { get; private set; } = Method.Get;

        /// <summary>
        /// http协议版本 默认1.1
        /// </summary>
        public Version Version { get; set; } = HttpVersion.Version11;

        /// <summary>
        /// 代理 默认无
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// 请求的Referer
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

        /// <summary>
        /// 固定端口代理
        /// </summary>
        public BindIPEndPoint BindIpEndPointDelegate { get; set; }
        #endregion

        /// <summary>
        /// 返回的头
        /// </summary>
        public WebHeaderCollection ResponseHeaders { get; set; }

        /// <summary>
        /// 返回的资源编码
        /// </summary>
        public Encoding Encod { get; set; }

        /// <summary>
        /// 开启自动重定向,默认开启
        /// </summary>
        /// <returns></returns>
        public HttpHead AllowAutoRedirectEnable()
        {
            AllowAutoRedirect = true;
            return this;
        }

        /// <summary>
        /// 关闭自动重定向,默认开启
        /// </summary>
        /// <returns></returns>
        public HttpHead AllowAutoRedirectDisable()
        {
            AllowAutoRedirect = false;
            return this;
        }

        /// <summary>
        /// 当前Head使用Cookie
        /// </summary>
        /// <returns></returns>
        public HttpHead CookieStateEnable()
        {
            CookieState = true;
            return this;
        }

        /// <summary>
        /// 当前Head禁用Cookie
        /// </summary>
        /// <returns></returns>
        public HttpHead CookieStateDisable()
        {
            CookieState = false;
            return this;
        }

        /// <summary>
        /// 添加请求头,若已存在则覆盖
        /// </summary>
        /// <param name="key">Http头Key</param>
        /// <param name="value">值</param>
        /// <returns>HttpHead</returns>
        public HttpHead AddRequestHeader(string key, string value)
        {
            var firstOrDefault = RequestHeaders.AllKeys.FirstOrDefault(c => c == key);
            if (!string.IsNullOrWhiteSpace(firstOrDefault)) RequestHeaders.Remove(key);
            RequestHeaders.Add(key, value);
            return this;
        }

        /// <summary>
        /// 设置ClientIp
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public HttpHead ClientIp(string ip)
        {
            return AddRequestHeader("Client-Ip", ip);
        }

        /// <summary>
        /// 设置RemoteAddr
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public HttpHead RemoteAddr(string ip)
        {
            return AddRequestHeader("RemoteAddr", ip);
        }

        /// <summary>
        /// 设置XForwardedFor
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public HttpHead XForwardedFor(string ip)
        {
            return AddRequestHeader("X-Forwarded-For", ip);
        }

        /// <summary>
        /// 同时设置ClientIp,RemoteAddr,XForwardedFor
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public HttpHead Ip(string ip)
        {
            return ClientIp(ip).RemoteAddr(ip).XForwardedFor(ip);
        }

        /// <summary>
        /// 设置支持的Accept-Encoding,默认支持gzip,deflate
        /// </summary>
        /// <param name="acceptEncoding"></param>
        /// <returns></returns>
        public HttpHead AcceptEncoding(string acceptEncoding)
        {
            return AddRequestHeader("Accept-Encoding", acceptEncoding);
        }

        /// <summary>
        /// 设置支持的Accept-Language,默认zh-CN,zh;q=0.8
        /// </summary>
        /// <param name="acceptlanguange"></param>
        /// <returns></returns>
        public HttpHead Acceptlanguange(string acceptlanguange)
        {
            return AddRequestHeader("Accept-Language", acceptlanguange);
        }

        /// <summary>
        /// 设置X-Requested-With请求方式,默认为普通
        /// </summary>
        /// <param name="xRequestedWith"></param>
        /// <returns></returns>
        public HttpHead SetXRequestedWith(XRequestedWith xRequestedWith)
        {
            if (xRequestedWith != XRequestedWith.Default) return AddRequestHeader("X-Requested-With", xRequestedWith.ToDescriptionName());

            var firstOrDefault = RequestHeaders.AllKeys.FirstOrDefault(c => c == "X-Requested-With");
            if (string.IsNullOrWhiteSpace(firstOrDefault)) return this;

            RequestHeaders.Remove("X-Requested-With");
            return this;
        }

        /// <summary>
        /// 设置Origin
        /// </summary>
        /// <param name="origin">origin</param>
        /// <returns>HttpHead</returns>
        public HttpHead Origin(string origin)
        {
            return AddRequestHeader("Origin", origin);
        }

        /// <summary>
        /// 通过Get获取网络资源
        /// </summary>
        /// <returns></returns>
        public DefaultData MethodGet()
        {
            Method = Method.Get;
            return new DefaultData(this);
        }

        /// <summary>
        /// 通过From方式Post获取网络资源
        /// </summary>
        /// <returns></returns>
        public DefaultData MethodPostDefault()
        {
            Method = Method.Post;
            ContentType = ContentType.Default;
            return new DefaultData(this);
        }

        /// <summary>
        /// 通过Json方式Post获取网络资源
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpHead MethodPostJson(Object data = null)
        {
            Method = Method.Post;
            ContentType = ContentType.Json;
            if (null != data) Data = data;
            return this;
        }

        /// <summary>
        /// 通过Xml方式Post获取网络资源
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpHead MethodPostXml(Object data = null)
        {
            Method = Method.Post;
            ContentType = ContentType.Xml;
            if (null != data) Data = data;
            return this;
        }

        /// <summary>
        /// 通过Stream方式Post获取网络资源
        /// </summary>
        /// <returns></returns>
        public StreamData MethodPostStream()
        {
            Method = Method.Post;
            ContentType = ContentType.Stream;
            return new StreamData(this);
        }

        /// <summary>
        /// 开启保持链接
        /// </summary>
        /// <returns></returns>
        public HttpHead KeepAliveEnable()
        {
            KeepAlive = true;
            return this;
        }

        /// <summary>
        /// 关闭保持链接
        /// </summary>
        /// <returns></returns>
        public HttpHead KeepAliveDisable()
        {
            KeepAlive = true;
            return this;
        }

        /// <summary>
        /// 普通get与post参数界定
        /// </summary>
        public class DefaultData
        {
            private readonly HttpHead _httpHead;
            private readonly NameValueCollection _data = new NameValueCollection();

            public DefaultData(HttpHead httpHead)
            {
                _httpHead = httpHead;
            }

            /// <summary>
            /// 增加请求参数
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public DefaultData AddData(string key, string value)
            {
                _data.Add(key, value);
                return this;
            }

            /// <summary>
            /// 结束参数增加
            /// </summary>
            /// <returns></returns>
            public HttpHead End()
            {
                _httpHead.Data = _data;
                return _httpHead;
            }
        }

        /// <summary>
        /// 流方式post参数界定
        /// </summary>
        public class StreamData
        {
            private readonly HttpHead _httpHead;
            private readonly NameValueCollection _data = new NameValueCollection();
            private readonly List<Tuple<string, string, byte[]>> _files = new List<Tuple<string, string, byte[]>>();

            public StreamData(HttpHead httpHead)
            {
                _httpHead = httpHead;
            }

            /// <summary>
            /// 增加请求参数
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public StreamData AddData(string key, string value)
            {
                _data.Add(key, value);
                return this;
            }

            /// <summary>
            /// 增加上传文件,默认key为filename
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="file"></param>
            /// <returns></returns>
            public StreamData AddFile(string fileName, byte[] file)
            {
                _files.Add(Tuple.Create("filename", fileName, file));
                return this;
            }

            /// <summary>
            /// 增加上传文件
            /// </summary>
            /// <param name="key"></param>
            /// <param name="fileName"></param>
            /// <param name="file"></param>
            /// <returns></returns>
            public StreamData AddFile(string key, string fileName, byte[] file)
            {
                _files.Add(Tuple.Create(key, fileName, file));
                return this;
            }

            public HttpHead End()
            {
                _httpHead.Data = _data;
                _httpHead.Files = _files;
                return _httpHead;
            }
        }
    }

    /// <summary>
    /// 请求方式
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Get
        /// </summary>
        [Description("GET")]
        Get = 0,

        /// <summary>
        /// Post
        /// </summary>
        [Description("POST")]
        Post = 1
    }

    /// <summary>
    /// 数据提交格式
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 普通From提交
        /// </summary>
        [Description("application/x-www-form-urlencoded; charset=utf-8")]
        Default = 0,

        /// <summary>
        /// Json提交
        /// </summary>
        [Description("application/json; charset=utf-8")]
        Json = 1,

        /// <summary>
        /// Xml提交
        /// </summary>
        [Description("application/xml; charset=UTF-8")]
        Xml = 2,

        /// <summary>
        /// 文件提交
        /// </summary>
        [Description("multipart/form-data; boundary=----------{0}")]
        Stream
    }

    /// <summary>
    /// 请求方式,默认普通
    /// </summary>
    public enum XRequestedWith
    {
        /// <summary>
        /// 普通请求
        /// </summary>
        [Description("")]
        Default = 0,

        /// <summary>
        /// Ajax等异步请求
        /// </summary>
        [Description("XMLHttpRequest")]
        Async = 1,
    }

    /// <summary>
    /// OpenHttp扩展
    /// </summary>
    public static class OpenHttpExpand
    {
        /// <summary>
        /// 请求的Url
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="url">url</param>
        /// <returns>Head对象</returns>
        public static HttpHead Url(this HttpHead head, string url)
        {
            head.Url = url;
            return head;
        }

        /// <summary>
        /// 希望接受的数据类型,默认text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="accept">数据类型</param>
        /// <returns></returns>
        public static HttpHead Accept(this HttpHead head, string accept)
        {
            head.Accept = accept;
            return head;
        }

        /// <summary>
        /// 自动重定向,默认开启
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="allowAutoRedirect"></param>
        /// <returns></returns>
        public static HttpHead AllowAutoRedirect(this HttpHead head, bool allowAutoRedirect)
        {
            head.AllowAutoRedirect = allowAutoRedirect;
            return head;
        }

        /// <summary>
        /// 设置Host
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="host">host</param>
        /// <returns>HttpHead</returns>
        public static HttpHead Host(this HttpHead head, string host)
        {
            head.Host = host;
            return head;
        }

        /// <summary>
        /// 设置最大重定向次数,默认50
        /// </summary>
        /// <param name="head"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static HttpHead MaximumAutomaticRedirections(this HttpHead head, int maximum)
        {
            head.MaximumAutomaticRedirections = maximum;
            return head;
        }

        /// <summary>
        /// 设置http协议版本 默认1.1
        /// </summary>
        /// <param name="head"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static HttpHead Version(this HttpHead head, Version version)
        {
            head.Version = version;
            return head;
        }

        /// <summary>
        /// 设置Proxy,默认无
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="proxy">proxy</param>
        /// <returns>HttpHead</returns>
        public static HttpHead Proxy(this HttpHead head, IWebProxy proxy)
        {
            head.Proxy = proxy;
            return head;
        }

        /// <summary>
        /// 设置Host
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="referer">referer</param>
        /// <returns>HttpHead</returns>
        public static HttpHead Referer(this HttpHead head, string referer)
        {
            head.Referer = referer;
            return head;
        }

        /// <summary>
        /// 设置UserAgent,默认Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36
        /// </summary>
        /// <param name="head"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static HttpHead UserAgent(this HttpHead head, string userAgent)
        {
            head.UserAgent = userAgent;
            return head;
        }

        /// <summary>
        /// 指定当前Head请求,本机使用的ip,端口,默认不设置
        /// </summary>
        /// <param name="head"></param>
        /// <param name="bindIpEndPoint"></param>
        /// <returns></returns>
        public static HttpHead BindIpEndPointDelegate(this HttpHead head, BindIPEndPoint bindIpEndPoint)
        {
            head.BindIpEndPointDelegate = bindIpEndPoint;
            return head;
        }

        /// <summary>
        /// 获取枚举的DescriptionAttribute属性
        /// </summary>
        /// <param name="enumeration">枚举值</param>
        /// <returns>DescriptionAttribute属性</returns>
        public static string ToDescriptionName(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var memInfo = type.GetMember(enumeration.ToString());
            if (memInfo.Length <= 0) return enumeration.ToString();
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumeration.ToString();
        }

        /// <summary>
#pragma warning disable 1570
        /// NameValueCollection转换为key=value&key=value
#pragma warning restore 1570
        /// </summary>
        public static string ToData(this NameValueCollection data)
        {
            var keyValueList = data.AllKeys.Select(key => new { key, keyValues = data.GetValues(key) })
                    .Where(t => t.keyValues != null)
                    .SelectMany(t => t.keyValues, (t, value) => $"{t.key}={value}");

            return string.Join("&", keyValueList);
        }

        /// <summary>
        /// 转换为Json,可以使用[JsonProperty("")]指定序列化字段名
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">待转换的Model</param>
        /// <returns>Json字符串</returns>
        public static string ToJson<T>(this T model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
        }

        /// <summary>
        /// 转换为Model,可以使用[JsonProperty("")]指定字段名绑定
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>Model</returns>
        public static T JsonToModel<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 转换为Xml
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">待转换的对象</param>
        /// <returns>Xml</returns>
        public static string ToXml<T>(this T model)
        {
            StringWriter sw = null;
            try
            {
                sw = new StringWriter(CultureInfo.InvariantCulture);
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(sw, model);
                var xml = sw.ToString();
                return xml;
            }
            finally
            {
                sw?.Close();
                sw?.Dispose();
            }
        }

        /// <summary>
        /// 转换为Model
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="xml">待转换的Xml</param>
        /// <returns>Model</returns>
        public static T XmlToModel<T>(this string xml)
        {
            StringReader sr = null;
            try
            {
                sr = new StringReader(xml);
                var ser = new XmlSerializer(typeof(T));
                var model = (T)ser.Deserialize(sr);
                return model;
            }
            finally
            {
                sr?.Close();
                sr?.Dispose();
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="byteArray">待转换Bytes</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>字符串</returns>
        public static string ToString(this byte[] byteArray, Encoding encoding)
        {
            return encoding.GetString(byteArray);
        }

        /// <summary>
        /// 转换为Bytes
        /// </summary>
        /// <param name="str">待转换字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>Bytes</returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 转换为Bytes
        /// </summary>
        /// <param name="image">待转换的Image</param>
        /// <returns>Bytes</returns>
        public static byte[] ToBytes(this Image image)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                var buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            finally
            {
                ms?.Close();
                ms?.Dispose();
            }
        }

        /// <summary>
        /// 转换为Image
        /// </summary>
        /// <param name="bytes">待转换的Bytes</param>
        /// <returns></returns>
        public static Image ToImage(this byte[] bytes)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(bytes);
                ms.Write(bytes, 0, bytes.Length);
                var image = Image.FromStream(ms, true);
                return image;
            }
            finally
            {
                ms?.Close();
                ms?.Dispose();
            }
        }

        public static IEnumerable<Cookie> ToCookies(this CookieContainer cc)
        {
            var table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, cc, new object[] { });
            return from object pathList in table.Values
                   select (SortedList)pathList.GetType().InvokeMember("m_list", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, pathList, new object[] { })
                   into lstCookieCol
                   from CookieCollection colCookies in lstCookieCol.Values
                   from Cookie c in colCookies
                   select c;
        }
    }

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
                    var getData = ((NameValueCollection)head.Data).ToData();

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
                        var defaultData = ((NameValueCollection)head.Data).ToData();
                        var defaultBytes = Encoding.UTF8.GetBytes(defaultData);

                        postBytes.Add(defaultBytes);
                        break;
                    case ContentType.Json:
                        var jsonData = head.Data.ToJson();
                        var jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                        postBytes.Add(jsonBytes);
                        break;
                    case ContentType.Xml:
                        var xmlData = head.ToXml();
                        var xmlBytes = Encoding.UTF8.GetBytes(xmlData);

                        postBytes.Add(xmlBytes);
                        break;
                    case ContentType.Stream:
                        #region 组装PostBytes
                        var boundary = DateTime.Now.Ticks.ToString("x");
                        myRequest.ContentType = string.Format(myRequest.ContentType, boundary);
                        var boundarybytes = Encoding.UTF8.GetBytes($"\r\n------------{boundary}\r\n");
                        var endbytes = Encoding.UTF8.GetBytes($"\r\n------------{boundary}--\r\n");

                        foreach (var data in ((NameValueCollection)head.Data).AllKeys.ToDictionary(c => c, c => ((NameValueCollection)head.Data).GetValues(c)))
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

            try
            {
                head.Encod = head.Encod ??
                             Encoding.GetEncoding(string.IsNullOrEmpty(myResponse.CharacterSet)
                                 ? "utf-8"
                                 : myResponse.CharacterSet);
            }
            catch
            {
                head.Encod = Encoding.UTF8;
            }
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
