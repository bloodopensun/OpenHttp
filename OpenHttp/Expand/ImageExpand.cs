using System.Drawing;
using System.IO;

namespace OpenHttp.Expand
{
    /// <summary>
    /// Image拓展
    /// </summary>
    public static class ImageExpand
    {
        /// <summary>
        /// 转换为Bytes
        /// </summary>
        /// <param name="image">待转换的Image</param>
        /// <returns>Bytes</returns>
        public static byte[] ToBytes(this Image image)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                var buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            finally
            {
                ms?.Close();
                ms?.Dispose();
            }
        }

        /// <summary>
        /// 转换为Image
        /// </summary>
        /// <param name="bytes">待转换的Bytes</param>
        /// <returns></returns>
        public static Image ToImage(this byte[] bytes)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(bytes);
                ms.Write(bytes, 0, bytes.Length);
                var image = Image.FromStream(ms, true);
                return image;
            }
            finally
            {
                ms?.Close();
                ms?.Dispose();
            }
        }
    }
}
