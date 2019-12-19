using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RawRabbitPublisher
{
    public interface IEventPublisher
    {
        Task PublishMessage<T>(T msg);
    }
    public class OutboxEventPublisher : IEventPublisher
    {
        public async Task PublishMessage<T>(T msg)
        {
            FakeDbContext.Messages.Add(new Message(msg));
            await Task.CompletedTask;
        }
    }
}
