# OpenHttp
由于工作原因以及个人爱好,经常需要抓取一些网络资源和调用一些非公开接口,使用了很多.Net自身组件,但并不太好用

并且本人对socket开发并不怎么熟悉,故只能在HttpWebRequest和HttpWebResponse做一些封装,使他们更好用(对我来说)

## 功能概述

- 轻量级(一个文件,上千行代码)
- 用于简单的Http/Https请求
- 使用链式的方式组装请求Head
- 自动识别压缩方式,自动解压
- 暂时只支持Get/Post请求

## 开源
下载

GitHub： https://github.com/bloodopensun/OpenHttp

## 代码介绍

### HttpClient类
```C#
public class HttpClient
{
	//提供一些优化的初始化配置
	static HttpClient()...

	//用来管理当前实例的cookie凭证
	public CookieContainer Cookie { get; set; } = new CookieContainer();

	//唯一一个对外方法,加载网络资源
	public byte[] Load(ref HttpHead head)...

	//url的一些处理
	private static string UrlPack(HttpHead head)...

	//对Httphead的一些处理
	private HttpWebRequest GetRequest(HttpHead head)...

	//Post的时候做一些处理
	private static HttpWebRequest PostPack(HttpWebRequest myRequest, HttpHead head)...

	//获取网络资源
	private HttpWebResponse GetResponse(HttpWebRequest myRequest, ref HttpHead head)
}
```

### HttpHead类
```C#
public class HttpHead
{

    //一些head属性
	...

    //开启自动重定向,默认开启
    public HttpHead AllowAutoRedirectEnable()...

    //关闭自动重定向,默认开启
    public HttpHead AllowAutoRedirectDisable()...

    //当前Head使用Cookie
    public HttpHead CookieStateEnable()...

    //当前Head禁用Cookie
    public HttpHead CookieStateDisable()...

    //添加请求头,若已存在则覆盖
    public HttpHead AddRequestHeader(string key, string value)...

    //设置ClientIp
    public HttpHead ClientIp(string ip)...

    //设置RemoteAddr
    public HttpHead RemoteAddr(string ip)...

    //设置XForwardedFor
    public HttpHead XForwardedFor(string ip)...

    //同时设置ClientIp,RemoteAddr,XForwardedFor
    public HttpHead Ip(string ip)...

    //设置支持的Accept-Encoding,默认支持gzip,deflate
    public HttpHead AcceptEncoding(string acceptEncoding)..

    //设置支持的Accept-Language,默认zh-CN,zh;q=0.8
    public HttpHead Acceptlanguange(string acceptlanguange)...

    //设置X-Requested-With请求方式,默认为普通
    public HttpHead SetXRequestedWith(XRequestedWith xRequestedWith)...

    //设置Origin
    public HttpHead Origin(string origin)..

    //设置通过Get获取网络资源
    public DefaultData MethodGet()..

    //设置通过From方式Post获取网络资源
    public DefaultData MethodPostDefault()...

    //设置通过Json方式Post获取网络资源
    public HttpHead MethodPostJson(Object data = null)...

    //设置通过Xml方式Post获取网络资源
    public HttpHead MethodPostXml(Object data = null)...

    //设置通过Stream方式Post获取网络资源
    public StreamData MethodPostStream()...

    //开启保持链接,默认开启
    public HttpHead KeepAliveEnable()..

    //关闭保持链接
    public HttpHead KeepAliveDisable()..

    //普通get与post参数界定
    public class DefaultData
    {
		...
    }

    //流方式post参数界定
    public class StreamData
    {
		...
    }
}
```

### 其他辅助类
```C#
//提供Head的链式编程扩展
public static class OpenHttpExpand
{
	...
}

public enum Method
{
	...
}

public enum ContentType
{
	...
}

public enum XRequestedWith
{
	...
}
```

## 怎么使用

### 下载单文件随便你放哪里
