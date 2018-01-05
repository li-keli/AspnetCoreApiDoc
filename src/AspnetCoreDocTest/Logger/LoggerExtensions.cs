using System;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest.Logger
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddESLogger(this ILoggerFactory factory, IServiceProvider serviceProvider, string indexName, FilterLoggerSettings filter = null)
        {
            factory.AddProvider(new ESLoggerProvider(serviceProvider, indexName, filter));
            return factory;
        }
    }
}