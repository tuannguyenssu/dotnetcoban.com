using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Application.Customers.Queries.GetCustomersList
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, CustomersListViewModel>
    {
        public async Task<CustomersListViewModel> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            var models = FakeDbContext.Customers;

            var vm = new CustomersListViewModel()
            {
                Customers = models.Select(c => new CustomerViewModelElement
                {
                    Name = c.Name,
                    Address = c.Address
                }).ToList()
            };

            return await Task.FromResult(vm);
        }
    }
}