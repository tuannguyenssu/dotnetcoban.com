using System.Threading.Tasks;
using System.Collections.Generic;
using GraphQLTest.Models;
using System.Linq;

namespace GraphQLTest.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        List<Customer> _customers = new List<Customer>()
        {
            new Customer
            {
                Id = 1,
                Name = "Tuan Nguyen 1",
                Address = "Vietnam"
            },
            new Customer
            {
                Id = 2,
                Name = "Tuan Nguyen 2",
                Address = "Vietnam"
            },
            new Customer
            {
                Id = 3,
                Name = "Tuan Nguyen 3",
                Address = "Vietnam"
            },                                
        };

        public Task<Customer> GetCustomerByIdAsync(int id)
        {
            return Task.FromResult(_customers.Single(o => Equals(o.Id, id)));
        }

        public Task<List<Customer>> GetCustomersAsync()
        {
            return Task.FromResult(_customers);
        }
    }
}