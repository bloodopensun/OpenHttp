using System.ComponentModel;

namespace OpenHttp.Enum
{
    /// <summary>
    /// 请求方式
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Get
        /// </summary>
        [Description("GET")]
        Get = 0,

        /// <summary>
        /// Post
        /// </summary>
        [Description("POST")]
        Post = 1
    }
}
