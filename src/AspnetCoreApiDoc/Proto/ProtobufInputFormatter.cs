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
    public class ProtobufInputFormatter : InputFormatter
    {
        public ProtobufInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
        }

        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public static RuntimeTypeModel Model => model.Value;

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService<ILogger<ProtobufInputFormatter>>();

            var type = context.ModelType;
            var request = context.HttpContext.Request;
            dynamic result = null;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);
            var url = request.Host.Value + request.Path.Value;
            var realIp = request.Headers["X-Real-IP"].ToString();
            try
            {
                result = Model.Deserialize(context.HttpContext.Request.Body, null, type);
                string jsonLog = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                Console.WriteLine($@"
【Request】：
【Time】：{DateTime.Now:yyyy-MM-dd HH:mm:ss}
【Session】：{context.HttpContext.TraceIdentifier}
【IP】：{realIp}
【Url】：{url}
【Data】：{jsonLog}
");
                logger.Log(LogLevel.Information, 200, (object) result, null, (state, error) => state.ToJson());
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

                logger.Log(LogLevel.Error, 200, "Proto反序列化异常：" + ex.Message, ex, (state, error) => state.ToJson());
                throw;
            }

            return InputFormatterResult.SuccessAsync(result);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }
    }
}