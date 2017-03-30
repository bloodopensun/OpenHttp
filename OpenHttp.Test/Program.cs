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