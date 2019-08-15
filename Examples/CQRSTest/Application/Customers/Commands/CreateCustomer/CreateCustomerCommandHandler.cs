using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
    {
        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            FakeDbContext.Customers.Add(new Customer
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address
            });
            
            return await Task.FromResult(Unit.Value);
        }
    }
}
