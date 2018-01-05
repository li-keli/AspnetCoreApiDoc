using System;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Filter;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspnetCoreApiDoc.Proto.Doc
{
    public class ApiGenOptions
    {
        internal IApiDocProvider CreateSwaggerProvider(IServiceProvider serviceProvider)
        {
            return new ApiGenerator(
                serviceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>(),
                serviceProvider.GetRequiredService<IXmlCommentsSchemaFilter>(),
                serviceProvider.GetRequiredService<IOptions<ApiDocSetting>>()
            );
        }
    }
}
