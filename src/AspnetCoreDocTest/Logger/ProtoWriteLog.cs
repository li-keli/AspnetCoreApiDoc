using System;
using System.ComponentModel;
using AspnetCoreApiDoc.Proto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest.Logger
{
   public class ProtoWriteLog
   {
       private readonly ILogger<ProtoWriteLog> _logger;

       private readonly Func<object, Exception, string> _messageFormatter = (state, error) => state.ToJson();

       public ProtoWriteLog(ILogger<ProtoWriteLog> logger)
       {
           _logger = logger;
       }

       /// <summary>
       /// API中间件记录日志
       /// </summary>
       /// <param name="obj">数据（类）</param>
       /// <param name="context">上下文</param>
       /// <param name="active">动作</param>
       /// <param name="ex">异常消息</param>
       public void WriteLog<T>(T obj, HttpContext context, ActiveType active, Exception ex = null)
       {
           var url = context.Request.Host.Value + context.Request.Path.Value;
           var realIp = context.Request.Headers["X-Real-IP"].ToString();

           string jsonLog = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
           Console.WriteLine($@"
【{active.GetDescription()}】：
【Time】：{DateTime.Now:yyyy-MM-dd HH:mm:ss}
【Session】：{context.TraceIdentifier}
【IP】：{realIp}
【Url】：{url}
【Data】：{jsonLog}
");
           _logger.Log(LogLevel.Information, new EventId(200, "日志"), obj, ex, _messageFormatter);
       }

       /// <summary>
       /// API中间件记录日志
       /// </summary>
       /// <param name="obj">数据（字符串）</param>
       /// <param name="context">上下文</param>
       /// <param name="active">动作</param>
       public void WriteLog(string obj, HttpContext context, ActiveType active)
       {
           Console.WriteLine($@"
【{active.GetDescription()}】：
【Time】：{DateTime.Now:yyyy-MM-dd HH:mm:ss}
【Session】：{context.TraceIdentifier}
【Url】：{context.Request.Host}
【Data】：{obj}
");
           _logger.Log(LogLevel.Information, new EventId(200, "日志"), obj, null, _messageFormatter);
       }

       public enum ActiveType
       {
           [Description("Request")] Request = 200,
           [Description("Response")] Response = 300,
           [Description("Response")] ErrorResponse = 404,
       }
   }
}