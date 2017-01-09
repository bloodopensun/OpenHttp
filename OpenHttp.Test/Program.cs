using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHttp.Test
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();
        static void Main(string[] args)
        {
            var head = HttpHead.Builder
                .Url("http://www.baidu.com")
                .MethodGet()
                    .AddData("wd", "GooGle")
                .End();
            var str = _httpClient.Load(ref head).ToString(head.Encod);
        }
    }
}
