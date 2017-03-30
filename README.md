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

## ��ôʹ��

### ���ص��ļ�����������

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