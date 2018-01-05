using Newtonsoft.Json;

namespace AspnetCoreApiDoc.Extensions
{
    internal static class JsonExtensions
    {
        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        internal static T JsonToObj<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
