# About

[![Build Status](https://travis-ci.com/li-keli/AspnetCoreApiDoc.svg?branch=master)](https://travis-ci.com/li-keli/AspnetCoreApiDoc)
[![Version Status](https://img.shields.io/badge/AspnetCoreApiDoc.version-v3.0.2-green.svg)](https://www.nuget.org/packages/AspnetCoreApiDoc)
[![NetCore Status](https://img.shields.io/badge/.Net%20core-v2.2-blue.svg)](https://github.com/li-keli/AspnetCoreApiDoc)
[![Platform Status](https://img.shields.io/badge/platform-windows%20%7C%20macos%20%7C%20linux-lightgrey.svg)](https://github.com/li-keli/AspnetCoreApiDoc)
[![top.996](https://img.shields.io/badge/link-top.996-red.svg)](https://github.com/top996/top.996)

API documents are automatically generated, used to generate development help documents on the APP side, and the default `ProtoBuffer` transmission format.

This project is not a `RESTful` style, but a function-oriented API type. The role of APiDoc is to automatically generate API docking documents for internal developers based on the defined API interfaces and comments.

# Nuget

`Install-Package AspnetCoreApiDoc`

# About ProtoBuffer

Official description：

> Protocol buffers are a language-neutral, platform-neutral extensible mechanism for serializing structured data.

# communicate with

* Bugs: [Issues](https://github.com/li-keli/AspnetCoreApiDoc/issues)
* Gitter: [Gitter channel](https://gitter.im/AspnetCoreApiDoc/AspNetCoreApiDoc)

# Generate documentation example

![Sample document](img/Sample-img.png)

![Prompt immediately after the online document is updated](img/2018-04-11_11.06.png)

![Prompt immediately after the online document is updated](img/2018-04-11_11.07.png)

# Documentation

**NO.1**

After quoting the project, add the following code to the `ConfigureServices` method in `Startup.cs` to register the service:

```c#
    //Register API document service
    services.AddProtoMvc(op =>
    {
        op.IsOpenDoc = true; // Open document access
        op.ApiOptions = new ApiOptions
        {
            //Route for API document access; recommended to be consistent with API address access
            DocRouter = "/core/v1",
            ApiName = "Sample API Document",
            APiVersion = "v1.0",
            Copyright = "Copyright©2018-2011 api.com All Rights Reserved. ",
            ProtoBufVersion = ProtoBufEnum.Proto3,
            NetworkDocs = new List<NetworkDoc>
            {
                new NetworkDoc
                {
                    Title = "Default Web Document One",
                    Url = "https://www.baidu.com/"
                },
                new NetworkDoc
                {
                    Title = "My Blog",
                    Url = "http://www.cnblogs.com/likeli/"
                },
            }
        };
        //Configure the ES log service address here
        //op.ESOptions = new ESOptions
        //{
        // Uri = "http://192.168.0.1:9200",
        // DefaultIndex = "test-log",
        //};
    });
```

**NO.2**

Enable the service in the `Configure` method:

```c#
    app.UseStatusCodePages()
        .UseApi(); //Enable API document generation
```

**NO.3**

Add the `ApiDoc` feature mark to the controller `Controller` or `method `Action` that needs to generate API documentation

E.g:

Add on `Controller`:
```c#
    [ApiDoc, Route("core/v1/[controller]/[action]/")]
    public class ApiController
    {
        ...
    }
```

Add on `Action`:
```c#
    /// <summary>
    /// Get product method 2
    /// </summary>
    /// <param name="input">input parameters</param>
    /// <returns>output parameters</returns>
    [ApiDoc, HttpPost]
    public ProductInput GetProduct2([FromBody] ProductInput input)
    {
        return new ProductInput {ProductName = "All-in-one machine"};
    }
```

After adding the ApiDoc feature to the controller, you can add `ApiDoc(false)` to the action under the controller to stop the document generation of a single method

**NO.4**

Add the configuration of generating XML to the `Project` node in the `.csproj` file of the API project and all its dependent projects, as follows:

```C#
  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <DocumentationFile>bin\Debug\netcoreapp2.0\{project name}.xml</DocumentationFile>
    <DocumentationFile>bin\Release\netcoreapp2.0\{project name}.xml</DocumentationFile>
    <NoWarn>1701;1702;1705;1591</NoWarn>
  </PropertyGroup>
```

**NO.5**

> Open `http://localhost:5000/core/v1/api.do` through the browser to access the API documentation

Full example:

```c#