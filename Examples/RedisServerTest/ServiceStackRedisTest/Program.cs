using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Common;
using ServiceStack.Redis;

namespace ServiceStackRedisTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestDbAsync();
            await TestPubSubAsync();
            Console.ReadKey();
        }

        public static async Task TestDbAsync()
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
        }

        private static async Task TestPubSubAsync()
        {
            var redisConnectionString = $"redis://192.168.1.57:6379?db=14";

            var redisClientManager = new PooledRedisClientManager(redisConnectionString);

            var redisPubSub = new RedisPubSubServer(redisClientManager, "Test");
            redisPubSub.OnMessage += (channel, message) =>
            {
                Console.WriteLine($"{channel} {message}");
            };
            redisPubSub.Start();
            var redisClient = await redisClientManager.GetClientAsync();
            for (int i = 0; i < 1000; i++)
            {
                var result = await redisClient.PublishMessageAsync("Test", "ChannelTest");
                Console.WriteLine(result);
            }
        }
    }
}
