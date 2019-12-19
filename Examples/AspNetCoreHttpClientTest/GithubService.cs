using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCoreHttpClientTest
{
    public class GithubService
    {
        // _httpClient isn't exposed publicly
        private readonly HttpClient _httpClient;

        public GithubService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<string> GetUserInfoAAsync()
        {
            var response = await _httpClient.GetAsync("user/tuannguyenssu");
            return JsonSerializer.Serialize(response);
        }
    }
}
