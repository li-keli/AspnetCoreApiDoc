using System;
using System.Reflection;

namespace AspnetCoreApiDoc.Filter
{
    public interface IXmlCommentsSchemaFilter
    {
        /// <summary>
        /// 获取类型文档描述
        /// </summary>
        string GetTypeDec(Type systemType);
        /// <summary>
        /// 获取方法文档描述
        /// </summary>
        string GetMethodDec(PropertyInfo systemType);
        /// <summary>
        /// 获取方法文档描述
        /// </summary>
        string GetFuncDec(MethodInfo methodInfo);
        /// <summary>
        /// 获取枚举注解描述
        /// </summary>
        string GetEnumDesc(Type enumType, string enumName);
    }
}