using System.Collections.Generic;

namespace AspnetCoreApiDoc.Constant
{
    /// <summary>
    /// API文档生成配置
    /// </summary>
    public class ApiDocSetting
    {
        /// <summary>
        /// ES日志服务器配置
        /// </summary>
        public ESOptions ESOptions { set; get; }

        /// <summary>
        /// API文档生成配置
        /// </summary>
        public ApiOptions ApiOptions { set; get; }
    }

    public class ESOptions
    {
        public string Uri { get; set; }
        public string DefaultIndex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ApiOptions
    {
        /// <summary>
        /// 访问的Host
        /// 例如访问地址为 /core/v1/api.do
        /// Host的值为：/core/v1;
        /// 推荐和API的访问路由一致
        /// </summary>
        public string Host { set; get; }

        /// <summary>
        /// API名称
        /// </summary>
        public string ApiName { set; get; }

        /// <summary>
        /// API版本
        /// </summary>
        public string APiVersion { set; get; }

        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright { set; get; }

        /// <summary>
        /// 编译状态地址(若是使用CI，此处则填写Build的svg地址)
        /// </summary>
        public string BuildSvg { set; get; }

        /// <summary>
        /// 代码覆盖率地址(若是使用CI，此处则填写Coverage的svg地址)
        /// </summary>
        public string CoverageSvg { set; get; }

        /// <summary>
        /// Proto版本<br/>
        /// 同时支持protobuf2和protobuf3<br/>
        /// 默认使用protobuf3
        /// </summary>
        public ProtoBufEnum ProtoBufVersion { set; get; } = ProtoBufEnum.Proto3;

        /// <summary>
        /// 更多的网络文档
        /// </summary>
        public List<NetworkDoc> NetworkDocs { set; get; } = new List<NetworkDoc>();
    }

    /// <summary>
    /// Protobuf版本
    /// </summary>
    public enum ProtoBufEnum
    {
        Proto2,
        Proto3,
    }

    /// <summary>
    /// 网络文档
    /// </summary>
    public class NetworkDoc
    {
        /// <summary>
        /// 文档标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { set; get; }
    }
}