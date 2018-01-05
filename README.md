# 关于

API文档自动生成，用于对APP端的开发帮助文档生成，默认`ProtoBuffer`传输格式

# Nuget下载

https://www.nuget.org/packages/AspnetCoreApiDoc/

`Install-Package AspnetCoreApiDoc`

# 生成文档示例

![示例文档](Sample-img.png)

# 说明文档

**NO.1**
引用项目后，在`Startup.cs`中的`ConfigureServices`方法加入如下代码，进行服务注册：

```c#
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
```

**NO.2**
在`Configure`方法启用服务：

```c#
    app.UseStatusCodePages()
        .UseApi();  //启用API文档生成
```

**NO.3**
在需要生成API文档的控制器`Controller``或`方法`Action`上添加`ApiDoc`特性标记

例如：

`Controller`上添加：
```c#
    [ApiDoc, Route("core/v1/[controller]/[action]/")]
    public class ApiController
    {
        ...
    }
```

`Action`上添加：
```c#
    /// <summary>
    /// 获取产品方法2
    /// </summary>
    /// <param name="input">输入参数</param>
    /// <returns>输出参数</returns>
    [ApiDoc, HttpPost]
    public ProductInput GetProduct2([FromBody] ProductInput input)
    {
        return new ProductInput {ProductName = "一体机"};
    }
```

在controller上添加ApiDoc特性后，可以在该控制器下的action上再添加`ApiDoc(false)`来停止某个单独方法的文档生成

完整实例：

```c#
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ESClientProvider>();
    
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
```

# 约定

* 所有API的方法传入参数必须从Body中读取

# 维护人

李科笠 （最后维护时间：2017年11月22日 15:23:33）