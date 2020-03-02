using System;
using CQRSTest.Application.Exceptions;
using CQRSTest.DataAccess;
using System.Collections.Generic;
using CQRSTest.Domain.Base;
using MediatR;

namespace CQRSTest.Domain.Customers
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer GetById(Guid customerId);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Guid customerId);

        void Save(Customer customer, int expectedVersion);

    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IEventStore _eventStore;

        public CustomerRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public List<Customer> GetAll()
        {
            return FakeDbContext.Customers;
        }

        public Customer GetById(Guid customerId)
        {
            Customer customer = FakeDbContext.Customers.Find(c => c.Id == customerId);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), customerId);
            }

            //var events = _eventStore.GetEventsForAggregate(customerId);
            return customer;
        }

        public void Add(Customer customer)
        {
            FakeDbContext.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            var index = FakeDbContext.Customers.FindIndex(c => c.Id == customer.Id);
            if (index < 0)
            {
                throw new NotFoundException(nameof(Customer), customer.Id);
            }
            FakeDbContext.Customers[index].Name = customer.Name;
            FakeDbContext.Customers[index].Address = customer.Address;
        }

        public void Delete(Guid customerId)
        {
            var model = FakeDbContext.Customers.Find(c => c.Id == customerId);
            if (model == null)
            {
                throw new NotFoundException(nameof(Customer), customerId);
            }

            FakeDbContext.Customers.Remove(model);
        }

        public void Save(Customer customer, int expectedVersion)
        {
            _eventStore.SaveEvents(customer.Id, customer.GetUncommittedChanges(), expectedVersion);
        }
    }
}
