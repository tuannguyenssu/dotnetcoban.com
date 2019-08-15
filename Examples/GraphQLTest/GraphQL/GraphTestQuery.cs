using GraphQL.Types;
using GraphQLTest.Models;
using GraphQLTest.Services;

namespace GraphQLTest.GraphQL
{
    public class GraphTestQuery : ObjectGraphType
    {
        public GraphTestQuery(ICustomerService customerService, IOrderService orderService)
        {
            Name = "Query";
            
            // Khai báo các hàm liên quan đến GET ở đây
            Field<ListGraphType<OrderType>>("orders", resolve : context => orderService.GetOrdersAsync());
            
            Field<ListGraphType<CustomerType>>("customers", resolve : context => customerService.GetCustomersAsync());

            FieldAsync<OrderType>(
            "orderById",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> {Name="orderId"}),
            resolve: async context => {
                return await context.TryAsyncResolve(
                    async c=> await orderService.GetOrderByIdAsync(c.GetArgument<string>("orderId"))
                );
            }
            );
        }
    }
}