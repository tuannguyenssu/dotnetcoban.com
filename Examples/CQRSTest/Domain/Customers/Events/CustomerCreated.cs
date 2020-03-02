using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSTest.Domain.Base;

namespace CQRSTest.Domain.Customers.Events
{
    public class CustomerCreated : Event
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
