using System;
using System.Collections.Generic;

namespace RawRabbitPublisher
{
    public class ProductCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public static class FakeDbContext
    {
        public static List<Message> Messages { get; set; } = new List<Message>();
    }
}
