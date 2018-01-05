using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Proto.Doc.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspnetCoreApiDoc.Proto.Doc
{
    public class ApiMiddleware
    {
        private readonly IApiDocProvider _apiProvider;
        private readonly RequestDelegate _next;
        private readonly JsonSerializer _apiSerializer;
        private readonly IOptions<ApiDocSetting> _setting;

        public const int BufferSize = 4096;
        private readonly string _uiHost;     // UI地址
        private readonly string _docHost;    // API数据地址
        private readonly string _wsHost;     // websocket地址

        public ApiMiddleware(
            RequestDelegate next,
            IApiDocProvider apiProvider,
            IOptions<ApiDocSetting> setting)
        {
            _next = next;
            _apiProvider = apiProvider;
            _setting = setting;
            _apiSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore, };

            _wsHost = setting.Value.ApiOptions.Host + "/ws";
            _uiHost = setting.Value.ApiOptions.Host + "/api.do";
            _docHost = setting.Value.ApiOptions.Host + "/doc";
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                await EchoLoop(httpContext);
            }
            if (httpContext.Request.Path.Value == _uiHost)
            {
                var uiStr = GetApiHtml.Get(_docHost, _wsHost, _setting.Value.ApiOptions.BuildSvg, _setting.Value.ApiOptions.CoverageSvg);
                httpContext.Response.ContentType = "text/html; charset=utf-8";
                await httpContext.Response.WriteAsync(uiStr);
                return;
            }
            if (httpContext.Request.Path.Value == _docHost && httpContext.Request.Method == "POST")
            {
                var protoBuf = PostInput(httpContext).Replace("=", string.Empty);
                switch (protoBuf)
                {
                    case "2":
                        _setting.Value.ApiOptions.ProtoBufVersion = ProtoBufEnum.Proto2;
                        break;
                    case "3":
                        _setting.Value.ApiOptions.ProtoBufVersion = ProtoBufEnum.Proto3;
                        break;
                }
                var apiDoc = _apiProvider.GetApi();
                RespondWithApiJson(httpContext.Response, apiDoc);
                return;
            }
            if (httpContext.Request.Headers["Accept"].ToString() != "application/x-protobuf")
            {
                // 过滤非proto请求
                httpContext.Response.StatusCode = 404;
                return;
            }

            await _next(httpContext);
        }

        private void RespondWithApiJson(HttpResponse response, ApiDocument swagger)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            using (var writer = new StreamWriter(response.Body, Encoding.GetEncoding("utf-8")))
            {
                _apiSerializer.Serialize(writer, swagger);
            }
        }

        private static async Task EchoLoop(HttpContext httpContext)
        {
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);
            while (socket.State == WebSocketState.Open)
            {
                var incoming = await socket.ReceiveAsync(seg, CancellationToken.None);
                var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                await socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private string PostInput(HttpContext context)
        {
            try
            {
                using (Stream s = context.Request.Body)
                {
                    int count = 0;
                    byte[] buffer = new byte[1024];
                    StringBuilder builder = new StringBuilder();
                    while ((count = s.Read(buffer, 0, 1024)) > 0)
                    {
                        builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}