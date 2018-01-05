using System;
using System.Collections.Generic;
using AspnetCoreApiDoc.Constant;
using Newtonsoft.Json;

namespace AspnetCoreApiDoc.Proto.Doc
{
    public class ApiDocument
    {
        public string ApiName { set; get; }

        public string Version { set; get; }

        public string Copyright { set; get; }

        public List<ApiGroup> ApiGroups { set; get; } = new List<ApiGroup>();

        public List<NetworkDoc> NetworkDocs { set; get; } = new List<NetworkDoc>();
    }

    public class ApiGroup
    {
        public string GroupName { set; get; }
        public List<DApiInfo> DApiInfo { set; get; } = new List<DApiInfo>();
    }

    public class DApiInfo
    {
        public string UUID { set; get; } = Guid.NewGuid().ToString("N");
        public string Url { set; get; }
        public string ApiName { set; get; }
        public string ApiDesc { set; get; }
        public ReqAndRes Request { set; get; }
        public string RequestProto { set; get; }
        public ReqAndRes Response { set; get; }
        public string ResponseProto { set; get; }
    }

    public class ReqAndRes
    {
        public string UUID { set; get; } = Guid.NewGuid().ToString("N");
        public string ClassName { set; get; }
        public string ClassDes { set; get; }
        public List<ParamItems> ParamItems { set; get; } = new List<ParamItems>();
    }
    public class ParamItems
    {
        public string UUID { set; get; } = Guid.NewGuid().ToString("N");
        public string Param { set; get; }
        public string ParamTypeName { set; get; }
        public string ParamTypeNameDec { set; get; }
        [JsonIgnore]
        public bool IsRequired { set; get; }
        public string IsRequiredStr => IsRequired ? "<i class='fa fa-check-square-o'></i>" : "";
        public object ClassObj { set; get; }
    }
}
