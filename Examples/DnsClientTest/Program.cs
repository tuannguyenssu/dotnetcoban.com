using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Protocol;

namespace DnsClientTest
{
    //https://dnsclient.michaco.net/
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new LookupClient();
            //string hostName = await client.GetHostNameAsync(IPAddress.Parse("8.8.8.8"));
            //Console.WriteLine(hostName);
            //var hostEntry = await client.GetHostEntryAsync("mail.google.com");
            //Console.WriteLine(hostEntry.AddressList);

            //var result = client.Query("dotnetcoban.com", QueryType.A).Answers.OfType<HInfoRecord>()
            //    .FirstOrDefault();
            //Console.WriteLine(JsonSerializer.Serialize(result));

            var record = (await client
                    .QueryAsync("dotnetcoban.com", QueryType.A))
                .Answers.OfRecordType(ResourceRecordType.HINFO)
                .FirstOrDefault() as HInfoRecord;
            if (record != null)
            {
                Console.WriteLine($"Cpu: {record.Cpu} OS: {record.OS}");
            }
            Console.ReadKey();
        }
    }
}
