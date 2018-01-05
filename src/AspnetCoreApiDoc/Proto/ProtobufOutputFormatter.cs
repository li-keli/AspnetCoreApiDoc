using System;
using System.Threading.Tasks;
using AspnetCoreApiDoc.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace AspnetCoreApiDoc.Proto
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public string ContentType { get; }

        public static RuntimeTypeModel Model => model.Value;

        public ProtobufOutputFormatter()
        {
            ContentType = "application/x-protobuf";
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));

            //SupportedEncodings.Add(Encoding.GetEncoding("utf-8"));
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService<ILogger<ProtobufOutputFormatter>>();
            var request = context.HttpContext.Request;
            var url = request.Host.Value + request.Path.Value;
            var realIp = request.Headers["X-Real-IP"].ToString();
            try
            {
                string jsonLog = Newtonsoft.Json.JsonConvert.SerializeObject(context.Object);
                Console.WriteLine($@"
【Request】：
【Time】：{DateTime.Now:yyyy-MM-dd HH:mm:ss}
【Session】：{context.HttpContext.TraceIdentifier}
【IP】：{realIp}
【Url】：{url}
【Data】：{jsonLog}
");
                logger.Log(LogLevel.Information, 300, context.Object, null, (state, error) => state.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"
【Request】：
【Time】：{DateTime.Now:yyyy-MM-dd HH:mm:ss}
【Session】：{context.HttpContext.TraceIdentifier}
【IP】：{realIp}
【Url】：{url}
【Data】：【Error => 】{ex.Message}
【Exception】:{ex.ToJson()}
");
                logger.Log(LogLevel.Error, 300, "Proto反序列化异常：" + ex.Message, ex, (state, error) => state.ToJson());
                throw;
            }

            var response = context.HttpContext.Response;

            Model.Serialize(response.Body, context.Object);
            return Task.FromResult(response);
        }
    }
}