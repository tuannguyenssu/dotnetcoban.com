using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Application.Exceptions;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var model = FakeDbContext.Customers.Find(c => c.Id == request.Id);
            if(model == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }
            
            FakeDbContext.Customers.Remove(model);

            return await Task.FromResult(Unit.Value);
        }
    }
}
