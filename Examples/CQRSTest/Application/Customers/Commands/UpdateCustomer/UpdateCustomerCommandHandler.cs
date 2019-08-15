using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Application.Exceptions;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var index = FakeDbContext.Customers.FindIndex(c => c.Id == request.Id);
            if(index < 0)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }
            FakeDbContext.Customers[index].Name = request.Name;
            FakeDbContext.Customers[index].Address = request.Address;
            
            return await Task.FromResult(Unit.Value);
        }
    }
}
