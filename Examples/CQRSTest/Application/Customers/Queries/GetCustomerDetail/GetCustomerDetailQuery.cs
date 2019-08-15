using MediatR;

namespace CQRSTest.Application.Customers.Queries.GetCustomerDetail
{
    public class GetCustomerDetailQuery : IRequest<CustomerDetailViewModel>
    {
        public int Id {get; set;}
    }
}