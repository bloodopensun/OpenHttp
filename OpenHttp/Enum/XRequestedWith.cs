using System.ComponentModel;

namespace OpenHttp.Enum
{
    /// <summary>
    /// 请求方式,默认普通
    /// </summary>
    public enum XRequestedWith
    {
        /// <summary>
        /// 普通请求
        /// </summary>
        [Description("")]
        Default = 0,

        /// <summary>
        /// Ajax等异步请求
        /// </summary>
        [Description("XMLHttpRequest")]
        Async = 1,
    }
}
