using System.Collections.Generic;
using CQRSTest.Domain;

namespace CQRSTest
{
    public class FakeDbContext
    {
        public static List<Customer> Customers {get; set;} = new List<Customer>()
            {
                new Customer
                {
                    Id = 1,
                    Name = "Tuan Nguyen",
                    Address = "Vietnam"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Nguyen Tuan",
                    Address = "Vietnam"
                }
            };
    }
}