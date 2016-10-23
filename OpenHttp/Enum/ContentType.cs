using System.ComponentModel;

namespace OpenHttp.Enum
{
    /// <summary>
    /// 数据提交格式
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 普通From提交
        /// </summary>
        [Description("application/x-www-form-urlencoded; charset=utf-8")]
        Default = 0,

        /// <summary>
        /// Json提交
        /// </summary>
        [Description("application/json; charset=utf-8")]
        Json = 1,

        /// <summary>
        /// Xml提交
        /// </summary>
        [Description("application/xml; charset=UTF-8")]
        Xml = 2,

        /// <summary>
        /// 文件提交
        /// </summary>
        [Description("multipart/form-data; boundary=----------{0}")]
        Stream
    }
}
