using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using ServiceStack.Redis;

namespace ServiceStackRedisTest
{
    public class ImageDataDb
    {
        private readonly ImageDataDbOptions _options;
        private readonly IRedisClientsManager _redisClientsManager;

        public ImageDataDb(ImageDataDbOptions options, IRedisClientsManager redisClientsManager)
        {
            _options = options;
            _redisClientsManager = redisClientsManager;
        }

        public async Task SaveAllAsync(List<ImageData> items)
        {
            await using var client = await _redisClientsManager.GetClientAsync();
            await using var pipeline = client.CreatePipeline();
            foreach (var item in items)
            {
                var key = $"{_options.DbName}:{item.CustomerId}:{item.VehicleName}";
                pipeline.QueueCommand(c => c.AddItemToSortedSetAsync(key, JsonSerializer.Serialize(item), item.CapturedTime.Ticks));
            }
            await pipeline.FlushAsync();
        }

        public async Task<List<ImageData>> GetAllAsync()
        {
            var result = new List<ImageData>();
            await using var client = await _redisClientsManager.GetClientAsync();
            var keys = await client.ScanAllKeysAsync($"{_options.DbName}:*", int.MaxValue).ToListAsync();
            await using var pipeline = client.CreatePipeline();
            foreach (var key in keys)
            {
                pipeline.QueueCommand(c => c.GetAllItemsFromSortedSetAsync(key), items =>
                {
                    foreach (var item in items)
                    {
                        result.Add(JsonSerializer.Deserialize<ImageData>(item));
                    }
                });
            }

            await pipeline.FlushAsync();
            return result;
        }
    }
}
