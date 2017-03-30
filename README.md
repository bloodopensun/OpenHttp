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

## 怎么使用

### 下载单文件随便你放哪里

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