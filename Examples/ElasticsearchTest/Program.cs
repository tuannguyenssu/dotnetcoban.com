using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Nest;

namespace ElasticsearchTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connString = "http://localhost:9200";

            var settings = new ConnectionSettings(new Uri(connString))
                .DefaultMappingFor<SearchProductModel>(i => i
                    .IndexName("product")
                )
                .PrettyJson();

            var client = new ElasticClient(settings);
            var products = new List<SearchProductModel>()
            {
                new SearchProductModel()
                {
                    Id = 1,
                    Name = "Product 1"
                },
                new SearchProductModel()
                {
                    Id = 2,
                    Name = "Product 2"
                },
                new SearchProductModel()
                {
                    Id = 3,
                    Name = "Product 3"
                }
            };


            for (var index = 0; index < products.Count; index++)
            {
                var response = await client.IndexDocumentAsync(products[index]);
                Console.WriteLine(response.IsValid);
            }

            var result = await client.SearchAsync<SearchProductModel>(s => s
                .Query(q => q
                    .MatchAll())
            );

            var request = new SearchRequest
            {
                From = 0,
                Size = 10,
                Query = new TermQuery { Field = "Id", Value = "1" } ||
                        new MatchQuery { Field = "Name", Query = "Product" }
            };

            Console.WriteLine(result.Hits.Count);
            var option = new JsonSerializerOptions();
            option.WriteIndented = true;
            Console.WriteLine(JsonSerializer.Serialize(result, option));

            Console.ReadKey();
        }
    }

    public class SearchProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
