using AspnetCoreApiDoc.Proto.Doc;
using Microsoft.AspNetCore.Builder;

namespace AspnetCoreApiDoc.Extensions
{
    public static class ApiBuilderExtensions
    {
        public static IApplicationBuilder UseApi(this IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseWebSockets();

            return app.UseMiddleware<ApiMiddleware>();
        }
    }
}