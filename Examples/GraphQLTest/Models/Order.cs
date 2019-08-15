

using System;

namespace GraphQLTest.Models
{
    [Flags]
    public enum OrderStatus
    {
        CREATED = 2,
        PROCESSING = 4,
        COMPLETED = 8,
        CANCELLED = 16,
        CLOSED = 32
    }
    public class Order 
    {
        public string Id {get; set;}
        public string Name {get; set;}

        public int CustomerId {get; set;}

        public OrderStatus Status {get; private set;} = OrderStatus.CREATED;

        public void Start()
        {
            if (Status != OrderStatus.CREATED) {
                throw new InvalidOperationException($"Order: {Id} cannot be started");
            }
            Status = OrderStatus.PROCESSING;
        }

        public void Complete() {
            if (Status != OrderStatus.PROCESSING) {
                throw new InvalidOperationException($"Order: {Id} cannot be completed");
            }

            Status = OrderStatus.COMPLETED;
        }

        public void Cancel() {
            if (Status == OrderStatus.CANCELLED || Status == OrderStatus.CLOSED || Status == OrderStatus.COMPLETED) {
                throw new InvalidOperationException($"Order: {Id} cannot be cancelled");
            }
            
            Status = OrderStatus.CANCELLED;
        }

        public void Close() {
            if (Status != OrderStatus.COMPLETED) {
                throw new InvalidOperationException($"Order: {Id} cannot be closed");
            }

            Status = OrderStatus.CLOSED;
        }
    }
}