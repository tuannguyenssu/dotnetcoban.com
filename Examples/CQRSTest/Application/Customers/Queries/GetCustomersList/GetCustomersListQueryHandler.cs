using CQRSTest.Domain.Customers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSTest.Application.Customers.Queries.GetCustomersList
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, CustomersListViewModel>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomersListQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomersListViewModel> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            var models = _repository.GetAll();

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