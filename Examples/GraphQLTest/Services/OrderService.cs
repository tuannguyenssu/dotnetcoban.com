using System.Threading.Tasks;
using System.Collections.Generic;
using GraphQLTest.Models;
using System.Linq;
using System;

namespace GraphQLTest.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> StartOrderAsync(string orderId);
        Task<Order> CompleteOrderAsync(string orderId);
        Task<Order> CancelOrderAsync(string orderId);
        Task<Order> CloseOrderAsync(string orderId);
    }

    public class OrderService : IOrderService
    {
        private static List<Order> _orders = new List<Order>()
        {
            new Order
            {
                Id = "faebd971-cba5-4ced-8ad5-cc0b8d4b7827",
                Name = "Books",
                CustomerId = 1
            },
            new Order
            {
                Id = "17ef9022-8d38-42f7-a21b-3a41847005fd",
                Name = "Fruits",
                CustomerId = 1
            },
            new Order
            {
                Id = "88f6d29c-4cdc-48df-b41c-d5801ec0b980",
                Name = "Cars",
                CustomerId = 2
            },                                
        };

        public Task<Order> CreateOrderAsync(Order order)
        {
            _orders.Add(order);
            return Task.FromResult(order);
        }

        public Task<Order> GetOrderByIdAsync(string id)
        {
            return Task.FromResult(_orders.Single(o => Equals(o.Id, id)));
        }

        public Task<List<Order>> GetOrdersAsync()
        {
            return Task.FromResult(_orders);
        }

        private Order GetById(string id)
        {
            var order = _orders.SingleOrDefault(o => Equals(o.Id, id));
            if (order == null)
            {
                throw new ArgumentException(string.Format("Order ID '{0}' is invalid", id));
            }
            return order;
        }

        public Task<Order> StartOrderAsync(string orderId)
        {
            var order = GetById(orderId);
            order.Start();
            return Task.FromResult(order);
        }                

        public Task<Order> CancelOrderAsync(string orderId)
        {
            var order = GetById(orderId);
            order.Cancel();
            return Task.FromResult(order);
        }

        public Task<Order> CloseOrderAsync(string orderId)
        {
            var order = GetById(orderId);
            order.Close();
            return Task.FromResult(order);            
        }

        public Task<Order> CompleteOrderAsync(string orderId)
        {
            var order = GetById(orderId);
            order.Complete();
            return Task.FromResult(order);        
        }        
    }
}