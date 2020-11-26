using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using ServiceStack.Redis;

namespace ServiceStackRedisTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var redisConnectionString = $"redis://192.168.1.57:6379?db=14";

            var redisClientManager = new PooledRedisClientManager(redisConnectionString);

            var options = new ImageDataDbOptions()
            {
                DbName = nameof(ImageDataDb)
            };

            var db = new ImageDataDb(options, redisClientManager);
            var data = new List<ImageData>()
            {
                new ImageData()
                {
                    CustomerId = 1010,
                    VehicleName = "BKSTEST",
                    CapturedTime = DateTime.Now
                }
            };

            await db.SaveAllAsync(data);

            var items = await db.GetAllAsync();

            Console.WriteLine(JsonSerializer.Serialize(items));
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
