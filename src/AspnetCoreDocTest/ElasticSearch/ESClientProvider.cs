using System;
using AspnetCoreApiDoc.Constant;
using AspnetCoreApiDoc.Model;
using AspnetCoreDocTest.Logger;
using Microsoft.Extensions.Options;
using Nest;

namespace AspnetCoreDocTest.ElasticSearch
{
    public class ESClientProvider
    {
        public ESClientProvider(IOptions<ApiDocSetting> options)
        {
            var settings = new ConnectionSettings(new Uri(options.Value.ESOptions.Uri))
                .ConnectionLimit(-1) //部署在centos上 需要这样写，不然异常 https://github.com/elastic/elasticsearch-net/issues/2758
                .DefaultIndex("default-log")
                .ThrowExceptions();

            if (!String.IsNullOrEmpty(options.Value.ESOptions.UserName) && !String.IsNullOrEmpty(options.Value.ESOptions.Password))
            {
                settings.BasicAuthentication(options.Value.ESOptions.UserName, options.Value.ESOptions.Password);
            }

            Client = new ElasticClient(settings);
            DefaultIndex = options.Value.ESOptions.DefaultIndex;
            EnsureIndexWithMapping<Product>(DefaultIndex);
        }

        public ElasticClient Client { get; }
        public string DefaultIndex { get; }

        public void EnsureIndexWithMapping<T>(string indexName = null, Func<PutMappingDescriptor<T>, PutMappingDescriptor<T>> customMapping = null) where T : class
        {
            try
            {
                if (String.IsNullOrEmpty(indexName)) indexName = DefaultIndex;

                // Map type T to that index
                Client.ConnectionSettings.DefaultIndices.Add(typeof(T), indexName);

                // Does the index exists?
                var indexExistsResponse = Client.IndexExists(new IndexExistsRequest(indexName));
                if (!indexExistsResponse.IsValid) throw new InvalidOperationException(indexExistsResponse.DebugInformation);

                // If exists, return
                if (indexExistsResponse.Exists) return;

                // Otherwise create the index and the type mapping
                var createIndexRes = Client.CreateIndex(indexName);
                if (!createIndexRes.IsValid) throw new InvalidOperationException(createIndexRes.DebugInformation);

                var res = Client.Map<T>(m =>
                {
                    m.AutoMap().Index(indexName);
                    if (customMapping != null) m = customMapping(m);
                    return m;
                });

                if (!res.IsValid) throw new InvalidOperationException(res.DebugInformation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ES访问日志异常：{ex.Message}");
            }
        }
    }
}
