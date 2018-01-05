using System;
using AspnetCoreApiDoc.Filter;
using AspnetCoreApiDoc.Proto.Doc;
using AspnetCoreApiDoc.Proto.Doc.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspnetCoreApiDoc.Extensions
{
    public static class ApiGenServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, Action<ApiGenOptions> setupAction = null)
        {
            services.Configure<MvcOptions>(c => c.Conventions.Add(new ApiDocApplicationConvention()));

            services.Configure(setupAction ?? (opts => { }));

            services.AddTransient<IXmlCommentsSchemaFilter, XmlCommentsSchemaFilter>();
            services.AddTransient(CreateApiProvider);

            return services;
        }

        private static IApiDocProvider CreateApiProvider(IServiceProvider serviceProvider)
        {
            var swaggerGenOptions = serviceProvider.GetRequiredService<IOptions<ApiGenOptions>>().Value;

            return swaggerGenOptions.CreateSwaggerProvider(serviceProvider);
        }
    }
}
