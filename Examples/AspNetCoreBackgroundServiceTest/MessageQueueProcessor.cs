using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AspNetCoreBackgroundServiceTest
{
    public class MessageQueueProcessor
    {
        private readonly ILogger<MessageQueueProcessor> _logger;
        private readonly Channel<int> _channel = Channel.CreateUnbounded<int>();
        private readonly Queue<int> _queue = new Queue<int>();

        public MessageQueueProcessor(ILogger<MessageQueueProcessor> logger)
        {
            _logger = logger;
        }

        public async Task AddDataAsync(int a)
        {
            await _channel.Writer.WriteAsync(a);
            _queue.Enqueue(a);
        }

        public async Task ProcessDataAsync()
        {
            var data = _channel.Reader.TakeAll().ToList();
            var data2 = _queue.TakeAll().ToList();
            _logger.LogInformation($"Channel Size {data.Count}");
            _logger.LogInformation($"Queue Size {data2.Count}");

            while (_queue.Count > 0)
            {
                _queue.Dequeue();
            }

            await Task.CompletedTask;
        }
    }

    public static class CollectionExtensions
    {
        public static IEnumerable<T> TakeAll<T>(this ChannelReader<T> reader)
        {
            while (reader.Count > 0)
            {
                if (reader.TryRead(out var item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> TakeAll<T>(this Queue<T> reader)
        {
            while (reader.Count > 0)
            {
                if (reader.TryDequeue(out var item))
                {
                    yield return item;
                }
            }
        }
    }
}
