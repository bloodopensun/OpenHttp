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

```C#
using System;

namespace OpenHttp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建HttpClient对象,各个HttpClient对象之间Cookie不共享
            //比如你要在A站点有多个账号要登录,请创建多个HttpClient对象
            var httpClient = new HttpClient();
            //HttpHead链式编程
            //尽量模拟正常访问,降低拒绝访问几率
            var head = HttpHead.Builder
                //当前请求不使用Cookie,此处不使用是模拟每次请求都是第一次
                .CookieStateDisable()
                //请求的地址
                .Url("http://www.baidu.com/s")
                //请求方式以及参数
                .MethodGet()
                    .AddData("wd", Uri.EscapeDataString("血开阳"))
                    .AddData("pn", "0")
                .End()
                //站点域名,此处设置后,Url可以使用Ip的方式访问
                //比如www.baidu.com有多个服务器,服务器a把你拉黑,你可以访问b
                .Host("www.baidu.com")
                //访问来源,很多站点做反外链,做检测爬虫都会读这个
                .Referer("http://www.baidu.com")
                //Ajax请求
                .SetXRequestedWith(XRequestedWith.Async)
                //自定义的头,抓包发现baidu有些这个,尽量模拟正常访问,降低拒绝访问几率
                .AddRequestHeader("is_xhr", "1")
                .AddRequestHeader("is_referer", "http://www.baidu.com")
                .AddRequestHeader("is_pbs", Uri.EscapeDataString("血开阳"));

            var html = httpClient.Load(ref head).ToString(head.Encod);
        }
    }
}
```
