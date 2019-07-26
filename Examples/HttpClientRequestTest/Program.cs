using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpClientRequestTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/");
            // GitHub API versioning
            client.DefaultRequestHeaders.Add("Accept",
                "application/vnd.github.v3+json");
            // GitHub requires a user-agent
            client.DefaultRequestHeaders.Add("User-Agent",
                "Test");
            string resource = "search/repositories?q=stars%3A>0&sort=stars&per_page=10";
            var response = await client.GetAsync(resource);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<GithubResponse>();
            foreach(var item in result.items)
            {
                Console.WriteLine(item.name);
            }

            Console.ReadKey();
        }
    }
}
