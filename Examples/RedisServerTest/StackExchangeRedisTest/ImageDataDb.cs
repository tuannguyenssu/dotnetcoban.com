using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Common;
using StackExchange.Redis;

namespace StackExchangeRedisTest
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

    public class ImageDataDb
    {
        private readonly ImageDataDbOptions _options;
        private readonly IDatabaseAsync _database;
        private readonly IServer _server;

        public ImageDataDb(ImageDataDbOptions options, IServer server, IDatabaseAsync database)
        {
            _options = options;
            _server = server;
            _database = database;
        }

        public async Task SaveAllAsync(List<ImageData> items)
        {
            var tasks = new List<Task>();

            foreach (var item in items)
            {
                var key = $"{_options.DbName}:{item.CustomerId}:{item.VehicleName}";
                var value = JsonSerializer.Serialize(item);
                var task = _database.SortedSetAddAsync(key, value, item.CapturedTime.Ticks);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        public async Task<List<ImageData>> GetAllAsync()
        {
            var result = new List<ImageData>();
            var keys = await _server.KeysAsync(14, $"{_options.DbName}:*", int.MaxValue).ToListAsync();
            foreach (var key in keys)
            {
                var items = await _database.SortedSetRangeByRankAsync(key, 0, int.MaxValue);
                foreach (var item in items)
                {
                    result.Add(JsonSerializer.Deserialize<ImageData>(item));
                }
            }

            return result;
        }
    }
}
