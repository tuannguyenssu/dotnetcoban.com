using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Application.Exceptions;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Application.Customers.Queries.GetCustomerDetail
{
    public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, CustomerDetailViewModel>
    {
        public async Task<CustomerDetailViewModel> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
        {
            // Ở đây chỉ giả lập dữ liệu. Trên thực tế phải tương tác với DB thật
            var model = FakeDbContext.Customers.Find(c => c.Id == request.Id);
            if(model == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }
            var vm = new CustomerDetailViewModel
            {
                Name = model.Name,
                Address = model.Address
            };
            return await Task.FromResult(vm);
            
        }
    }
}