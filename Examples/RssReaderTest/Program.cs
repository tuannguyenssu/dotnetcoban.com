using System;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Toolkit.Parsers.Rss;

namespace RssReaderTest
{
    //https://mitchelsellers.com/blog/article/creating-an-rss-feed-in-asp-net-core-3-0
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestRssParser();
            TestSyndicationParser();
            Console.ReadKey();
        }

        private static void TestSyndicationParser()
        {
            var rssUrl = "https://visualstudiomagazine.com/rss-feeds/news.aspx";
            using var reader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(reader);
            foreach (var item in feed.Items)
            {
                //Console.WriteLine();
                //Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions()
                //{
                //    WriteIndented = true,
                //    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                //}));
                Console.WriteLine(item.Id);
            }

        }

        private static async Task TestRssParser()
        {
            //var rssUrl = "https://visualstudiomagazine.com/rss-feeds/news.aspx";
            //var rssUrl = "https://arminreiter.com/feed";
            //var rssUrl = "https://andrewlock.net/rss.xml";
            //var rssUrl = "https://ericlippert.com/feed";
            //var rssUrl = "https://www.reddit.com/r/csharp/.rss?format=xml";
            var rssUrl = "https://ericlippert.com/feed";
            
            using var client = new HttpClient();
            var s = await client.GetStringAsync(rssUrl);
            var parser = new RssParser();
            var rss = parser.Parse(s);
            foreach (var item in rss)
            {
                Console.WriteLine();
                Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            }
        }
    }
}
