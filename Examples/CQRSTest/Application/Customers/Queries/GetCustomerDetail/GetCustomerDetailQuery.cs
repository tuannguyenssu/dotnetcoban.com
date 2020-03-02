using MediatR;

namespace CQRSTest.Application.Customers.Queries.GetCustomerDetail
{
    public class GetCustomerDetailQuery : IRequest<CustomerDetailViewModel>
    {
        public string Id {get; set;}
    }
}