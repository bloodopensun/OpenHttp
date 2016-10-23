using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace OpenHttp.Expand
{
    public static class CookieContainerExpand
    {
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
}
