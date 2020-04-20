using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticsearchTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var urlList = new List<string>()
            {
                "http://localhost:9200"
            };

            var uris = urlList.Select(u => new Uri(u));
            var pool = new StaticConnectionPool(uris);
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            var indexTemplateName = $"test-logs-template";
            var indexPatterns = $"test-logs-*";
            var indexAlias = $"test-logs";
            var indexName = $"test-logs-v1";

            var indexRepository = new IndexRepository(client);
            var repository = new DeviceLogRepository(client, indexName);

            // Xoa index va index template trong truong hop da ton tai
            //await indexRepository.DeleteIndexAsync(indexName);
            //await indexRepository.DeleteIndexTemplateAsync(indexTemplateName);

            // Tao moi mot index
            var isCreateIndexSuccess = await indexRepository.CreateIndexWithTemplateAsync(indexTemplateName, indexPatterns, indexName, indexAlias);
            if (!isCreateIndexSuccess)
            {
                Console.WriteLine("Tao index khong thanh cong!");
            }

            // Index du lieu
            var log = new DeviceLog()
            {
                CustomerId = 8888,
                VehiclePlate = "29A12345",
                CreatedTime = DateTime.UtcNow,
                Message = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };

            var isIndexSuccess = await repository.SaveAsync(log);
            if (!isIndexSuccess)
            {
                Console.WriteLine("Index du lieu khong thanh cong!");
            }

            Thread.Sleep(1000);

            // Tim kiem du lieu
            //var logs = await repository.ListAllAsync();
            var logs = await repository.SearchAsync("8888");

            var json = JsonSerializer.Serialize(logs, new JsonSerializerOptions() {WriteIndented = true});

            Console.WriteLine(json);

            // Xoa tat ca cac du lieu
            //await repository.DeleteAllAsync();

            Console.ReadKey();
        }
    }
}
