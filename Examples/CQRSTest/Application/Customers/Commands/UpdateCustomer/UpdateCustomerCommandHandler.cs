using System;
using CQRSTest.Domain.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSTest.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _repository.GetById(Guid.Parse(request.Id));
            if (customer == null) return Task.FromResult(false);
            _repository.Update(customer.Update(request));
            return Task.FromResult(true);
        }
    }
}
