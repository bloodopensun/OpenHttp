using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using OpenHttp.Enum;

namespace OpenHttp.Expand
{
    /// <summary>
    /// HttpHead扩展
    /// </summary>
    public static class HttpHeadExpand
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
        /// 开启自动重定向,默认开启
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns></returns>
        public static HttpHead AllowAutoRedirectEnable(this HttpHead head)
        {
            head.AllowAutoRedirect = true;
            return head;
        }

        /// <summary>
        /// 关闭自动重定向,默认开启
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns></returns>
        public static HttpHead AllowAutoRedirectDisable(this HttpHead head)
        {
            head.AllowAutoRedirect = false;
            return head;
        }

        /// <summary>
        /// 设置请求的参数列表,调用此方法会覆盖AddData方法加入的参数
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="data">参数列表</param>
        /// <returns></returns>
        public static HttpHead Data(this HttpHead head, NameValueCollection data)
        {
            head.Data = data;
            return head;
        }

        /// <summary>
        /// 添加请求参数
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="key">参数名</param>
        /// <param name="value">值</param>
        /// <returns>HttpHead</returns>
        public static HttpHead AddData(this HttpHead head, string key, string value)
        {
            head.Data.Add(key, value);
            return head;
        }

        /// <summary>
        /// 添加上传文件,key默认为filename,并自动设置ContentType为Stream,Method为Post
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static HttpHead AddFile(this HttpHead head, string fileName, byte[] file)
        {
            return head.AddFile("filename", fileName, file);
        }

        /// <summary>
        /// 添加上传文件,并自动设置ContentType为Stream,Method为Post
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="key">参数名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>HttpHead</returns>
        public static HttpHead AddFile(this HttpHead head, string key, string fileName, byte[] file)
        {
            head = head.MethodPost().ContentType(Enum.ContentType.Stream);
            head.Files.Add(Tuple.Create(key, fileName, file));
            return head;
        }

        /// <summary>
        /// 设置 Post数据方式
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="type">Post数据类型</param>
        /// <returns></returns>
        public static HttpHead ContentType(this HttpHead head, ContentType type)
        {
            head.ContentType = type;
            return head;
        }

        /// <summary>
        /// 当前Head使用Cookie
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="cookieState">Cookie状态</param>
        /// <returns></returns>
        public static HttpHead CookieState(this HttpHead head, bool cookieState)
        {
            head.CookieState = cookieState;
            return head;
        }

        /// <summary>
        /// 当前Head使用Cookie
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns></returns>
        public static HttpHead CookieStateEnable(this HttpHead head)
        {
            head.CookieState = true;
            return head;
        }

        /// <summary>
        /// 当前Head禁用Cookie
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns></returns>
        public static HttpHead CookieStateDisable(this HttpHead head)
        {
            head.CookieState = false;
            return head;
        }

        /// <summary>
        /// 添加请求头,若已存在则覆盖
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="key">Http头Key</param>
        /// <param name="value">值</param>
        /// <returns>HttpHead</returns>
        public static HttpHead AddRequestHeader(this HttpHead head, string key, string value)
        {
            var firstOrDefault = head.RequestHeaders.AllKeys.FirstOrDefault(c => c == key);
            if (!string.IsNullOrWhiteSpace(firstOrDefault))
            {
                head.RequestHeaders.Remove(key);
            }
            head.RequestHeaders.Add(key, value);
            return head;
        }

        /// <summary>
        /// 设置ClientIp
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static HttpHead ClientIp(this HttpHead head, string ip)
        {
            return head.AddRequestHeader("Client-Ip", ip);
        }

        /// <summary>
        /// 设置RemoteAddr
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static HttpHead RemoteAddr(this HttpHead head, string ip)
        {
            return head.AddRequestHeader("RemoteAddr", ip);
        }

        /// <summary>
        /// 设置XForwardedFor
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static HttpHead XForwardedFor(this HttpHead head, string ip)
        {
            return head.AddRequestHeader("X-Forwarded-For", ip);
        }

        /// <summary>
        /// 同时设置ClientIp,RemoteAddr,XForwardedFor
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static HttpHead Ip(this HttpHead head, string ip)
        {
            return head.ClientIp(ip).RemoteAddr(ip).XForwardedFor(ip);
        }

        /// <summary>
        /// 设置支持的Accept-Encoding,默认支持gzip,deflate
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="acceptEncoding"></param>
        /// <returns></returns>
        public static HttpHead AcceptEncoding(this HttpHead head, string acceptEncoding)
        {
            return head.AddRequestHeader("Accept-Encoding", acceptEncoding);
        }

        /// <summary>
        /// 设置支持的Accept-Language,默认zh-CN,zh;q=0.8
        /// </summary>
        /// <param name="head"></param>
        /// <param name="acceptlanguange"></param>
        /// <returns></returns>
        public static HttpHead Acceptlanguange(this HttpHead head, string acceptlanguange)
        {
            return head.AddRequestHeader("Accept-Language", acceptlanguange);
        }

        /// <summary>
        /// 设置X-Requested-With请求方式,默认为普通
        /// </summary>
        /// <param name="head"></param>
        /// <param name="xRequestedWith"></param>
        /// <returns></returns>
        public static HttpHead XRequestedWith(this HttpHead head, XRequestedWith xRequestedWith)
        {
            if (xRequestedWith != Enum.XRequestedWith.Default) return head.AddRequestHeader("X-Requested-With", xRequestedWith.ToDescriptionName());

            var firstOrDefault = head.RequestHeaders.AllKeys.FirstOrDefault(c => c == "X-Requested-With");
            if (string.IsNullOrWhiteSpace(firstOrDefault)) return head;

            head.RequestHeaders.Remove("X-Requested-With");
            return head;
        }

        /// <summary>
        /// 设置Origin
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="origin">origin</param>
        /// <returns>HttpHead</returns>
        public static HttpHead Origin(this HttpHead head, string origin)
        {
            return head.AddRequestHeader("Origin", origin);
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
        /// 设置是否保持链接
        /// </summary>
        /// <param name="head"></param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpHead KeepAlive(this HttpHead head, bool keepAlive)
        {
            head.KeepAlive = keepAlive;
            return head;
        }

        /// <summary>
        /// 开启保持链接
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static HttpHead KeepAliveEnable(this HttpHead head)
        {
            return head.KeepAlive(true);
        }

        /// <summary>
        /// 关闭保持链接
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static HttpHead KeepAliveDisable(this HttpHead head)
        {
            return head.KeepAlive(false);
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
        /// 设置Method扩展
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <param name="method">method</param>
        /// <returns>HttpHead</returns>
        public static HttpHead Method(this HttpHead head, Method method)
        {
            head.Method = method;
            return head;
        }

        /// <summary>
        /// 设置Method扩展
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns>HttpHead</returns>
        public static HttpHead MethodGet(this HttpHead head)
        {
            return head.Method(Enum.Method.Get);
        }

        /// <summary>
        /// 设置Method扩展
        /// </summary>
        /// <param name="head">Head对象</param>
        /// <returns>HttpHead</returns>
        public static HttpHead MethodPost(this HttpHead head)
        {
            return head.Method(Enum.Method.Post);
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
    }
}
