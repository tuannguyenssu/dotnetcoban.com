using System;
using MediatR;

namespace CQRSTest.Domain.Base
{
    public class Event : INotification
    {
        public Guid Id;
        public int Version;
        public Event()
        {
            Id = Guid.NewGuid();
        }
    }
}