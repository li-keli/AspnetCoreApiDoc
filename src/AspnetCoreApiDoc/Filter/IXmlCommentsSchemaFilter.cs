using System;
using System.Reflection;

namespace AspnetCoreApiDoc.Filter
{
    public interface IXmlCommentsSchemaFilter
    {
        string GetTypeDec(Type systemType);
        string GetMethodDec(PropertyInfo systemType);
        string GetFuncDec(MethodInfo methodInfo);
        string GetEnumDesc(Type enumType, string enumName);
    }
}