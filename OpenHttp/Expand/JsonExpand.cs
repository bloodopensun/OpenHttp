using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenHttp.Expand
{
    public static class JsonExpand
    {
        /// <summary>
        /// 转换为Json,可以使用[JsonProperty("")]指定序列化字段名
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">待转换的Model</param>
        /// <returns>Json字符串</returns>
        public static string ToJson<T>(this T model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
        }

        /// <summary>
        /// 转换为Model,可以使用[JsonProperty("")]指定字段名绑定
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>Model</returns>
        public static T JsonToModel<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
