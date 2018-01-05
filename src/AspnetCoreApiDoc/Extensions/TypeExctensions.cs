using System;
using System.Reflection;

namespace AspnetCoreApiDoc.Extensions
{
    internal static class TypeExctensions
    {
        /// <summary>
        /// 是否为用户构建的类
        /// </summary>
        internal static bool IsUserClass(this Type t)
        {
            if (t.GetTypeInfo().IsPrimitive)
            {
                //是否基元类型
                //包括：Boolean、 Byte、 SByte、 Int16、 UInt16、 Int32、 UInt32、 Int64、 UInt64、 Char、 Double和 Single
                return false;
            }

            return t != typeof(string) && t != typeof(DateTime);
        }
    }
}
