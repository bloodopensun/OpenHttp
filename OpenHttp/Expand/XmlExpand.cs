using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace OpenHttp.Expand
{
    /// <summary>
    /// Xml拓展
    /// </summary>
    public static class XmlExpand
    {
        /// <summary>
        /// 转换为Xml
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">待转换的对象</param>
        /// <returns>Xml</returns>
        public static string ToXml<T>(this T model)
        {
            StringWriter sw = null;
            try
            {
                sw = new StringWriter(CultureInfo.InvariantCulture);
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(sw, model);
                var xml = sw.ToString();
                return xml;
            }
            finally
            {
                sw?.Close();
                sw?.Dispose();
            }
        }

        /// <summary>
        /// 转换为Model
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="xml">待转换的Xml</param>
        /// <returns>Model</returns>
        public static T XmlToModel<T>(this string xml)
        {
            StringReader sr = null;
            try
            {
                sr = new StringReader(xml);
                var ser = new XmlSerializer(typeof(T));
                var model = (T)ser.Deserialize(sr);
                return model;
            }
            finally
            {
                sr?.Close();
                sr?.Dispose();
            }
        }
    }
}
