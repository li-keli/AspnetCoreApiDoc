using System;
using System.Linq;
using System.Reflection;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Extensions;
using AspnetCoreApiDoc.Filter;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using ProtoBuf;
using ProtoBuf.Meta;

namespace AspnetCoreApiDoc.Proto.Doc
{
    public interface IApiDocProvider
    {
        ApiDocument GetApi();
    }

    public class ApiGenerator : IApiDocProvider
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionsProvider;
        private readonly IXmlCommentsSchemaFilter _xmlFilter;
        private readonly IOptions<ApiDocSetting> _options;

        public ApiGenerator(IApiDescriptionGroupCollectionProvider apiDescriptionsProvider,
            IXmlCommentsSchemaFilter xmlFilter, IOptions<ApiDocSetting> options)
        {
            _apiDescriptionsProvider = apiDescriptionsProvider;
            _xmlFilter = xmlFilter;
            _options = options;
        }

        public ApiDocument GetApi()
        {
            var actionGroups = _apiDescriptionsProvider.ApiDescriptionGroups.Items;


            var dapi = new ApiDocument
            {
                ApiName = _options.Value.ApiOptions.ApiName,
                Version = _options.Value.ApiOptions.APiVersion,
                Copyright = _options.Value.ApiOptions.Copyright,
                NetworkDocs = _options.Value.ApiOptions.NetworkDocs
            };

            foreach (var actionGroup in actionGroups)
            {
                if (actionGroup == null) continue;
                var apiGroup = new ApiGroup {GroupName = actionGroup.GroupName};

                foreach (var apiDescription in actionGroup.Items)
                {
                    var dapiInfo = new DApiInfo();
                    var routeTemplate = apiDescription.RelativePath;
                    var requestParam = apiDescription.ParameterDescriptions.FirstOrDefault();
                    var responseParam = apiDescription.SupportedResponseTypes.FirstOrDefault();
                    var requestClass = requestParam == null
                        ? null
                        : (requestParam.Type == typeof(void) ? null : Activator.CreateInstance(requestParam.Type));
                    var responseClass = responseParam == null
                        ? null
                        : (responseParam?.Type == typeof(void) ? null : Activator.CreateInstance(responseParam?.Type));
                    var methinfo = (apiDescription.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;

                    dapiInfo.Url = routeTemplate.ToLower();
                    dapiInfo.ApiName = routeTemplate.Split('/').Last().ToLower();
                    dapiInfo.ApiDesc = _xmlFilter.GetFuncDec(methinfo);
                    dapiInfo.RequestProto = GetProtoFile(requestClass);
                    dapiInfo.ResponseProto = GetProtoFile(responseClass);
                    dapiInfo.Request = GetParamInfo(requestParam?.Type);
                    dapiInfo.Response = GetParamInfo(responseParam?.Type);

                    apiGroup.DApiInfo.Add(dapiInfo);
                }
                dapi.ApiGroups.Add(apiGroup);
            }


            return dapi;
        }

        private string GetProtoFile(object obj)
        {
            if (obj == null) return "";

            var protoStr = string.Empty;
            try
            {
                switch (_options.Value.ApiOptions.ProtoBufVersion)
                {
                    case ProtoBufEnum.Proto2:
                        protoStr = RuntimeTypeModel.Default.GetSchema(obj.GetType(), ProtoSyntax.Proto2);
                        break;
                    case ProtoBufEnum.Proto3:
                        protoStr = RuntimeTypeModel.Default.GetSchema(obj.GetType(), ProtoSyntax.Proto3);
                        break;
                    default:
                        protoStr = RuntimeTypeModel.Default.GetSchema(obj.GetType(), ProtoSyntax.Proto3);
                        break;
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

            return protoStr;
        }

        private ReqAndRes GetParamInfo(Type t)
        {
            if (t == null) return new ReqAndRes();
            ReqAndRes rp = new ReqAndRes();
            var properties = t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (var pro in properties)
            {
                //去掉非proto特性的属性
                if (pro.CustomAttributes.Count(ct => ct.AttributeType == typeof(ProtoMemberAttribute)
                                                     || ct.AttributeType == typeof(ProtoContractAttribute)) ==
                    0) continue;

                ParamItems pi = new ParamItems();
                var methodName = pro.Name;
                var returnType = GetListTypeName(pro.PropertyType);
                rp.ClassName = t.Name;
                rp.ClassDes = _xmlFilter.GetTypeDec(t);
                pi.Param = methodName;
                pi.ParamTypeName = returnType;
                pi.ParamTypeNameDec = _xmlFilter.GetMethodDec(pro);

                //默认都是必须的字段
                var isR = pro.CustomAttributes
                    .FirstOrDefault(p => p.AttributeType == typeof(ProtoMemberAttribute))
                    ?.NamedArguments
                    .FirstOrDefault(p => p.MemberName == "IsRequired").TypedValue.Value;
                pi.IsRequired = (bool?) isR ?? true;

                if (pro.PropertyType.IsUserClass())
                {
                    if (pro.PropertyType.GetTypeInfo().IsGenericType)
                    {
                        var tt = pro.PropertyType.GetGenericArguments();
                        pi.ClassObj = GetParamInfo(tt.First());
                    }
                    else if (pro.PropertyType.GetTypeInfo().IsEnum)
                    {
                        pi.ClassObj = GetEnumInfo(pro, pro.PropertyType);
                    }
                    else
                    {
                        pi.ClassObj = GetParamInfo(pro.PropertyType);
                    }
                }

                rp.ParamItems.Add(pi);
            }
            return rp;
        }

        private ReqAndRes GetEnumInfo(PropertyInfo p, Type t)
        {
            ReqAndRes rp = new ReqAndRes();
            var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Static);
            var memberInfos = t.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fieldInfos)
            {
                ParamItems pi = new ParamItems();
                rp.ClassName = t.Name;
                rp.ClassDes = _xmlFilter.GetTypeDec(t);
                pi.Param = field.Name;
                pi.ParamTypeName = ((int) field.GetValue(p)).ToString();
                pi.ParamTypeNameDec = _xmlFilter.GetEnumDesc(field.FieldType, field.Name);
                rp.ParamItems.Add(pi);
            }
            return rp;
        }

        private static string GetListTypeName(Type t)
        {
            return t.GetTypeInfo().IsGenericType
                ? $"List<{t.GetGenericArguments().First().Name}>"
                : t.Name;
        }
    }
}