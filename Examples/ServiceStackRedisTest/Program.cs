using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace ServiceStackRedisTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var redisConnectionString = $"redis://192.168.1.57:6379?db=15";

            var redisClientManager = new PooledRedisClientManager(redisConnectionString);

            var redisClient = await redisClientManager.GetClientAsync();
            var options = new ImageDataDbOptions()
            {
                DbName = "Abc"
            };

            var db = new ImageDataDb(redisClient, options);
            var data = new List<ImageData>()
            {
                new ImageData()
                {
                    CustomerId = 1010,
                    VehicleName = "BKSTEST",
                    CapturedTime = DateTime.Now
                }
            };

            //await db.SaveAllAsync(data);

            var items = await db.GetAllAsync();

            Console.WriteLine(items.Count);
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
