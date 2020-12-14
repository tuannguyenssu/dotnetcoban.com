using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;

namespace AngleSharpTest
{
    public class Article
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            var blogs = new List<string>()
            {
                "viblo.asia",
                "topdev.vn"
            };
            var url = "https://topdup.xyz";
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            var document = await context.OpenAsync(url);

            var items = document.QuerySelectorAll("#all_posts_table > tbody > tr");
            foreach (var item in items)
            {
                var columns = item.QuerySelectorAll("td");
                var dataInfo = columns[1];
                var baseUrlInfo = columns[2].TextContent.Trim();
                var timeInfo = columns[3];

                if (blogs.Any(x => x.Contains(baseUrlInfo)))
                {
                    var link = $"{url}{dataInfo.QuerySelector("a").Attributes["href"].Value}";
                    var detailDocument = await context.OpenAsync(link);
                    var post = detailDocument.QuerySelector("table tbody tr td a");
                    Console.WriteLine();
                    var article = new Article()
                    {
                        Url = post.Attributes["href"].Value,
                        Title = post.TextContent,
                        CreatedTime = DateTime.Parse(timeInfo.TextContent.Trim())
                    };
                    Console.WriteLine(JsonSerializer.Serialize(article, new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }));
                }
            }

            Console.ReadKey();

        }
    }
}
