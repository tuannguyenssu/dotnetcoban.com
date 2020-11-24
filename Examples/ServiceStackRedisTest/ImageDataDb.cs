using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace ServiceStackRedisTest
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken)
                .ConfigureAwait(false))
                results.Add(item);
            return results;
        }
    }

    public class ImageDataDbOptions
    {
        public string DbName { get; set; } = nameof(ImageDataDb);
    }
    public class ImageDataDb
    {
        private readonly ImageDataDbOptions _options;
        private readonly IRedisClientAsync _redisClient;

        public ImageDataDb(IRedisClientAsync redisClient, ImageDataDbOptions options)
        {
            _redisClient = redisClient;
            _options = options;
        }

        public async Task SaveAllAsync(List<ImageData> items)
        {
            var pipeline = _redisClient.CreatePipeline();
            foreach (var item in items)
            {
                var key = $"{_options.DbName},{item.CustomerId},{item.VehicleName}";
                pipeline.QueueCommand(c => c.AddItemToSortedSetAsync(key, JsonSerializer.Serialize(item), item.CapturedTime.Ticks));
            }
            await pipeline.FlushAsync();
        }

        public async Task<List<ImageData>> GetAllAsync()
        {
            var result = new List<ImageData>();
            //var keys = _redisClient.ScanAllKeysAsync($"{_options.DbName},*", int.MaxValue);
            var keys = await _redisClient.ScanAllKeysAsync($"{_options.DbName},*", int.MaxValue).ToListAsync();
            var items = await _redisClient.GetValuesAsync(keys);
            return result;
        }
    }
}
