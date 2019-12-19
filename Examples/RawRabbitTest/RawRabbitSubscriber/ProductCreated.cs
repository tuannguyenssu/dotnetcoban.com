using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace RawRabbitSubscriber
{
    public class ProductCreated : INotification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
