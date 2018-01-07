using System.Collections.Generic;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Extensions;
using AspnetCoreApiDoc.Proto;
using AspnetCoreDocTest.ElasticSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspnetCoreDocTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //加载日志记录组件
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<ESClientProvider>();

            //注册API文档服务
            services.AddProtoMvc(op =>
            {
                op.ApiOptions = new ApiOptions
                {
                    //API文档访问的路由; 推荐和API地址访问保持一致
                    Host = "/core/v1",
                    ApiName = "样例API文档",
                    APiVersion = "v1.0",
                    Copyright = "Copyright©2017-2018 api.com All Rights Reserved. ",
                    ProtoBufVersion = ProtoBufEnum.Proto3,
                    //BuildSvg = "",
                    //CoverageSvg = "",
                    NetworkDocs = new List<NetworkDoc>
                    {
                        new NetworkDoc
                        {
                            Title = "默认网络文档一",
                            Url = "https://www.baidu.com/"
                        },
                        new NetworkDoc
                        {
                            Title = "我的博客",
                            Url = "http://www.cnblogs.com/likeli/"
                        },
                    }
                };
                //此处配置ES日志服务地址
                //op.ESOptions = new ESOptions
                //{
                //    Uri = "http://192.168.0.1:9200",
                //    DefaultIndex = "test-log",
                //};
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //启动ES日志服务
            //loggerFactory
            //    .AddESLogger(app.ApplicationServices, "test-log", new FilterLoggerSettings
            //    {
            //        {"*", LogLevel.Trace},
            //        {"Microsoft", LogLevel.Warning},
            //        {"System", LogLevel.Warning},
            //    });
            app.UseStatusCodePages()
                .UseApi(); //启用API文档生成
        }
    }
}
