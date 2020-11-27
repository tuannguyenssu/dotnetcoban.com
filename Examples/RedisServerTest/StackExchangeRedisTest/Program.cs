using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using StackExchange.Redis;

namespace StackExchangeRedisTest
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
            var redisConnectionString = $"192.168.1.57:6379";

            var options = new ImageDataDbOptions()
            {
                DbName = nameof(ImageDataDb)
            };

            var redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);

            var database = redis.GetDatabase(14);
            var server = redis.GetServer(redis.GetEndPoints().First());

            var db = new ImageDataDb(options, server, database);
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
            var redisConnectionString = $"192.168.1.57:6379";

            var redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);

            var redisPubSub = redis.GetSubscriber();

            redisPubSub.Subscribe("Test").OnMessage(message =>
            {
                Console.WriteLine(message);
            });

            redisPubSub.Publish("Test", "Abc");
        }
    }
}
