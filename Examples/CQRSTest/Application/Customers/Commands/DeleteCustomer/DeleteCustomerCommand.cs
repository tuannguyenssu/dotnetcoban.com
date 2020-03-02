using MediatR;

namespace CQRSTest.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
