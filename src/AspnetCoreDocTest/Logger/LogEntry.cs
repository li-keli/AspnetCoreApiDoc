using System;
using Microsoft.AspNetCore.Http;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AspnetCoreDocTest.Logger
{
    public class LogEntry
    {
        [Date]
        public DateTime DateTime { get; set; }

        [Keyword]
        [JsonConverter(typeof(StringEnumConverter))]
        public Microsoft.Extensions.Logging.LogLevel Level { get; set; }

        [Keyword]
        public string Category { get; set; }

        [Object]
        public object Message { get; set; }

        [Keyword]
        public string ActiveType { set; get; }

        [Keyword]
        public string TraceIdentifier { get; set; }

        [Keyword]
        public string UserName { get; set; }

        [Keyword]
        public string ContentType { get; set; }

        [Keyword]
        public string Host { get; set; }

        [Keyword]
        public string Method { get; set; }

        [Keyword]
        public string Protocol { get; set; }

        [Keyword]
        public string Scheme { get; set; }

        public string Path { get; set; }
        public string PathBase { get; set; }
        public long? ContentLength { get; set; }
        public bool IsHttps { get; set; }
        public IHeaderDictionary Headers { get; set; }

        [Ip]
        public string Ip { set; get; }

        [Keyword]
        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }
        public string Exception { get; set; }
        public bool HasException => Exception != null;
        public string StackTrace { get; set; }
    }
}