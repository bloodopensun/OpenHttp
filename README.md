# OpenHttp
���ڹ���ԭ���Լ����˰���,������ҪץȡһЩ������Դ�͵���һЩ�ǹ����ӿ�,ʹ���˺ܶ�.Net�������,������̫����

���ұ��˶�socket����������ô��Ϥ,��ֻ����HttpWebRequest��HttpWebResponse��һЩ��װ,ʹ���Ǹ�����(������˵)

## ���ܸ���

- ������(һ���ļ�,��ǧ�д���)
- ���ڼ򵥵�Http/Https����
- ʹ����ʽ�ķ�ʽ��װ����Head
- �Զ�ʶ��ѹ����ʽ,�Զ���ѹ
- ��ʱֻ֧��Get/Post����

## ��Դ
����

GitHub�� https://github.com/bloodopensun/OpenHttp

## �������

### HttpClient��
```C#
public class HttpClient
{
	//�ṩһЩ�Ż��ĳ�ʼ������
	static HttpClient()...

	//��������ǰʵ����cookieƾ֤
	public CookieContainer Cookie { get; set; } = new CookieContainer();

	//Ψһһ�����ⷽ��,����������Դ
	public byte[] Load(ref HttpHead head)...

	//url��һЩ����
	private static string UrlPack(HttpHead head)...

	//��Httphead��һЩ����
	private HttpWebRequest GetRequest(HttpHead head)...

	//Post��ʱ����һЩ����
	private static HttpWebRequest PostPack(HttpWebRequest myRequest, HttpHead head)...

	//��ȡ������Դ
	private HttpWebResponse GetResponse(HttpWebRequest myRequest, ref HttpHead head)
}
```

### HttpHead��
```C#
public class HttpHead
{

    //һЩhead����
	...

    //�����Զ��ض���,Ĭ�Ͽ���
    public HttpHead AllowAutoRedirectEnable()...

    //�ر��Զ��ض���,Ĭ�Ͽ���
    public HttpHead AllowAutoRedirectDisable()...

    //��ǰHeadʹ��Cookie
    public HttpHead CookieStateEnable()...

    //��ǰHead����Cookie
    public HttpHead CookieStateDisable()...

    //�������ͷ,���Ѵ����򸲸�
    public HttpHead AddRequestHeader(string key, string value)...

    //����ClientIp
    public HttpHead ClientIp(string ip)...

    //����RemoteAddr
    public HttpHead RemoteAddr(string ip)...

    //����XForwardedFor
    public HttpHead XForwardedFor(string ip)...

    //ͬʱ����ClientIp,RemoteAddr,XForwardedFor
    public HttpHead Ip(string ip)...

    //����֧�ֵ�Accept-Encoding,Ĭ��֧��gzip,deflate
    public HttpHead AcceptEncoding(string acceptEncoding)..

    //����֧�ֵ�Accept-Language,Ĭ��zh-CN,zh;q=0.8
    public HttpHead Acceptlanguange(string acceptlanguange)...

    //����X-Requested-With����ʽ,Ĭ��Ϊ��ͨ
    public HttpHead SetXRequestedWith(XRequestedWith xRequestedWith)...

    //����Origin
    public HttpHead Origin(string origin)..

    //����ͨ��Get��ȡ������Դ
    public DefaultData MethodGet()..

    //����ͨ��From��ʽPost��ȡ������Դ
    public DefaultData MethodPostDefault()...

    //����ͨ��Json��ʽPost��ȡ������Դ
    public HttpHead MethodPostJson(Object data = null)...

    //����ͨ��Xml��ʽPost��ȡ������Դ
    public HttpHead MethodPostXml(Object data = null)...

    //����ͨ��Stream��ʽPost��ȡ������Դ
    public StreamData MethodPostStream()...

    //������������,Ĭ�Ͽ���
    public HttpHead KeepAliveEnable()..

    //�رձ�������
    public HttpHead KeepAliveDisable()..

    //��ͨget��post�����綨
    public class DefaultData
    {
		...
    }

    //����ʽpost�����綨
    public class StreamData
    {
		...
    }
}
```

### ����������
```C#
//�ṩHead����ʽ�����չ
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

## ��ôʹ��

### ���ص��ļ�����������

```C#
using System;

namespace OpenHttp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //����HttpClient����,����HttpClient����֮��Cookie������
            //������Ҫ��Aվ���ж���˺�Ҫ��¼,�봴�����HttpClient����
            var httpClient = new HttpClient();
            //HttpHead��ʽ���
            //����ģ����������,���;ܾ����ʼ���
            var head = HttpHead.Builder
                //��ǰ����ʹ��Cookie,�˴���ʹ����ģ��ÿ�������ǵ�һ��
                .CookieStateDisable()
                //����ĵ�ַ
                .Url("http://www.baidu.com/s")
                //����ʽ�Լ�����
                .MethodGet()
                    .AddData("wd", Uri.EscapeDataString("Ѫ����"))
                    .AddData("pn", "0")
                .End()
                //վ������,�˴����ú�,Url����ʹ��Ip�ķ�ʽ����
                //����www.baidu.com�ж��������,������a��������,����Է���b
                .Host("www.baidu.com")
                //������Դ,�ܶ�վ����������,��������涼������
                .Referer("http://www.baidu.com")
                //Ajax����
                .SetXRequestedWith(XRequestedWith.Async)
                //�Զ����ͷ,ץ������baidu��Щ���,����ģ����������,���;ܾ����ʼ���
                .AddRequestHeader("is_xhr", "1")
                .AddRequestHeader("is_referer", "http://www.baidu.com")
                .AddRequestHeader("is_pbs", Uri.EscapeDataString("Ѫ����"));

            var html = httpClient.Load(ref head).ToString(head.Encod);
        }
    }
}
```
