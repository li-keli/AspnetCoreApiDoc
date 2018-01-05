using System.IO;
using System.Text;
using ProtoBuf;

namespace AspnetCoreApiDoc.Extensions
{
    public static class ProtobufExtensions
    {
        /// <summary>
        /// 序列化
        /// </summary>
        public static string ToProto<T>(this T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public static byte[] ToProtoByte<T>(this T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, t);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static T ProtoToObj<T>(this string content)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                return Serializer.Deserialize<T>(ms);
        }
    }
}
