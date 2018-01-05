using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AspnetCoreApiDoc.Extensions
{
    internal static class EnumExtensions
    {
        /// <summary>
        /// 读取特性描述
        /// </summary>
        internal static string GetDescription(this Enum e)
        {
            Type type = e.GetType();
            MemberInfo[] memInfo = type.GetMember(e.ToString());
            
            if (null == memInfo || memInfo.Length <= 0) return "";
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).ToList();
            if (null != attrs && attrs.Count > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
            return "";
        }
    }
}
