using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace RawRabbitPublisher
{
    public class Outbox
    {
        private readonly IBusClient _busClient;
        private ILogger<Outbox> _logger;

        public Outbox(IBusClient busClient, ILogger<Outbox> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        private IList<Message> FetchPendingMessages()
        {
            if (FakeDbContext.Messages.Count <= 0)
            {
                var product = new ProductCreated()
                {
                    Id = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString()
                };
                FakeDbContext.Messages.Add(new Message(product));
            }
            List<Message> messagesToPush = FakeDbContext.Messages.Take(50).ToList();
            return messagesToPush;
        }
        private async Task PublishMessage(Message msg)
        {
            var deserializedMsg = msg.RecreateMessage();
            var messageKey = deserializedMsg.GetType().Name.ToLower();
            await _busClient.BasicPublishAsync(deserializedMsg,
                cfg =>
                {
                    cfg.OnExchange("rabbit-test").WithRoutingKey(messageKey);
                });
        }

        private async Task<bool> TryPush(Message msg)
        {
            try
            {
                await PublishMessage(msg);
                FakeDbContext.Messages.RemoveAll(m => m.Id == msg.Id);
                _logger.LogInformation("Successfully pushed message");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to push message from outbox", null);
                return false;
            }
        }

        public async Task PushPendingMessages()
        {
            var messagesToPush = FetchPendingMessages();
            _logger.LogInformation($"{messagesToPush.Count()} messages about to be pushed.");

            foreach (var msg in messagesToPush)
            {
                if (!await TryPush(msg))
                    break;
            }
        }
    }
}
