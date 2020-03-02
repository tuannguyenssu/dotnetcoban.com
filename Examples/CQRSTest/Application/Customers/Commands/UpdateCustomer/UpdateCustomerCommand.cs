using MediatR;

namespace CQRSTest.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
