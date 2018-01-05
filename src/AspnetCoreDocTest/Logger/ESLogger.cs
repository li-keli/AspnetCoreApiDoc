using System;
using AspnetCoreDocTest.ElasticSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest.Logger
{
    public class ESLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ESClientProvider _esClient;
        private readonly string _categoryName;
        private readonly LogLevel _logLevel;

        public ESLogger(ESClientProvider esClient, IHttpContextAccessor httpContextAccessor, string categoryName, LogLevel logLevel)
        {
            _esClient = esClient;
            _httpContextAccessor = httpContextAccessor;
            _categoryName = categoryName;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // Outside scope of article
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                if (!IsEnabled(logLevel)) return;

                var message = formatter(state, exception);
                var entry = new LogEntry
                {
                    DateTime = DateTime.UtcNow,
                    Category = _categoryName,
                    Message = message,
                    Level = logLevel,
                };

                switch (eventId.Id)
                {
                    case 200:
                        entry.ActiveType = "Request";
                        break;
                    case 300:
                        entry.ActiveType = "Response";
                        break;
                    default:
                        entry.ActiveType = "None";
                        break;
                }

                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    entry.TraceIdentifier = context.TraceIdentifier;
                    entry.UserName = context.User.Identity.Name;
                    var request = context.Request;
                    entry.ContentLength = request.ContentLength;
                    entry.ContentType = request.ContentType;
                    entry.Host = request.Host.Value;
                    entry.IsHttps = request.IsHttps;
                    entry.Method = request.Method;
                    entry.Path = request.Path;
                    entry.PathBase = request.PathBase;
                    entry.Protocol = request.Protocol;
                    entry.Scheme = request.Scheme;

                    entry.Ip = request.Headers["X-Real-IP"].ToString();
                    entry.Headers = request.Headers;
                }

                if (exception != null)
                {
                    entry.Exception = exception.ToString();
                    entry.ExceptionMessage = exception.Message;
                    entry.ExceptionType = exception.GetType().Name;
                    entry.StackTrace = exception.StackTrace;
                }

                _esClient.Client.Index(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ES访问日志记录异常：{ex.Message}");
            }
        }
    }
}