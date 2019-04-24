using AspnetCoreDocTest;
using AspnetCoreDocTest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AspnetCoreDocUnitTest
{
    public class ApiTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const string Host = "core/v1/api/";
        private readonly ITestOutputHelper _output;

        public ApiTest(ITestOutputHelper output)
        {
            _output = output;
            _server = new TestServer(
                new WebHostBuilder()
                    .UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        [Fact]
        public async Task GetProduct()
        {
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(stream, new ProductInput());

            HttpContent data = new StreamContent(stream);

            var response = _client.PostAsync(Host + "GetProduct", data).Result;
            var result = ProtoBuf.Serializer.Deserialize<ProductInput>(await response.Content.ReadAsStreamAsync());

            Assert.True(response.IsSuccessStatusCode);

            var jsonResult = JsonConvert.SerializeObject(result);
            _output.WriteLine(jsonResult);
        }
    }
}
