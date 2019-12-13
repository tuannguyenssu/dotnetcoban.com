using GraphQL.Types;
using GraphQLTest.Models;
using GraphQLTest.Services;

namespace GraphQLTest.GraphQL
{
    public class OrderType : ObjectGraphType<Order>
    {
        public OrderType(ICustomerService customers)
        {
            Field(o => o.Id);
            Field(o => o.Name);
            Field<CustomerType>("customer",
                resolve: context => customers.GetCustomerByIdAsync(context.Source.CustomerId));
            Field<OrderStatusesEnum>("status",
                resolve: context => context.Source.Status);
        }
    }
}