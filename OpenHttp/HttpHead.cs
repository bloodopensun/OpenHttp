using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using OpenHttp.Enum;

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
        public NameValueCollection Data { get; set; } = new NameValueCollection();

        /// <summary>
        /// 上传的文件
        /// </summary>
        public List<Tuple<string, string, byte[]>> Files { get; } = new List<Tuple<string, string, byte[]>>();

        /// <summary>
        /// Post数据方式
        /// </summary>
        public ContentType ContentType { get; set; } = ContentType.Default;

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
        public Method Method { get; set; } = Method.Get;

        /// <summary>
        /// http协议版本 默认1.1
        /// </summary>
        public Version Version { get; set; } = HttpVersion.Version11;

        /// <summary>
        /// 代理 默认无
        /// </summary>
        public IWebProxy Proxy { get; set; } = null; 

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
    }
}
