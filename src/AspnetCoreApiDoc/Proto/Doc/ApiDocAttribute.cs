using System;

namespace AspnetCoreApiDoc.Proto.Doc
{
    /// <summary>
    /// API生成标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiDocAttribute : Attribute
    {
        /// <summary>
        /// 是否生成文档
        /// </summary>
        public bool IsCreateDoc { set; get; }

        /// <summary>
        /// 分组名称 默认以控制器名称为组
        /// </summary>
        public string GroupName { set; get; }

        /// <summary>
        /// API生成标记
        /// </summary>
        /// <param name="groupName">分组名称 默认以控制器名称为组</param>
        /// <param name="isCreateDoc">是否生成文档，默认生成</param>
        public ApiDocAttribute(string groupName = null, bool isCreateDoc = true)
        {
            IsCreateDoc = isCreateDoc;
            GroupName = groupName;
        }
    }
}