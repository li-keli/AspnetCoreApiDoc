using System;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace AspnetCoreApiDoc.Proto
{
    public static class ProtobufOperation
    {
        public static void AddProtoMvc(this IServiceCollection services, Action<ApiDocSetting> acop, Action<MvcOptions> setupAction = null)
        {
            ApiDocSetting op = new ApiDocSetting();
            acop?.Invoke(op);
            services.Configure(acop ?? (opts => { }));

            if (setupAction != null)
            {
                services.AddMvc(setupAction);
            }
            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new ProtobufInputFormatter());
                options.OutputFormatters.Add(new ProtobufOutputFormatter());
                options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf", MediaTypeHeaderValue.Parse("application/x-protobuf"));
            });
            services.AddApi();
        }
    }
}