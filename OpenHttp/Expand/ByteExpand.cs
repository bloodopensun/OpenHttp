using System.Text;

namespace OpenHttp.Expand
{
    /// <summary>
    /// Byte扩展
    /// </summary>
    public static class ByteExpand
    {
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="byteArray">待转换Bytes</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>字符串</returns>
        public static string ToString(this byte[] byteArray, Encoding encoding)
        {
            return encoding.GetString(byteArray);
        }

        /// <summary>
        /// 转换为Bytes
        /// </summary>
        /// <param name="str">待转换字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>Bytes</returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }
    }
}
