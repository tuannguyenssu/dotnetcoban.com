using MediatR;

namespace CQRSTest.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
