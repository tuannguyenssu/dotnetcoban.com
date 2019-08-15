using System;
using GraphQL.Types;
using GraphQLTest.Models;
using GraphQLTest.Services;

namespace GraphQLTest.GraphQL
{
    public class GraphTestMutation : ObjectGraphType
    {
        public GraphTestMutation(IOrderService orderService)
        {
            Name = "Mutation";
            // Khai báo các hàm liên quan đến POST, PUT, PATCH, DELETE ở đây
            Field<OrderType>(
                "createOrder",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<OrderCreateInputType>> { Name = "order" }),
                resolve: context =>
                {
                    var orderInput = context.GetArgument<OrderCreateInput>("order");
                    var id = Guid.NewGuid().ToString();
                    var order = new Order
                    {
                        Id = id,
                        Name = orderInput.Name,
                        CustomerId = orderInput.CustomerId
                    };
                    return orderService.CreateOrderAsync(order);
                }
            );

            FieldAsync<OrderType>(
                "startOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await orderService.StartOrderAsync(orderId));
                }
            );

            FieldAsync<OrderType>(
                "completeOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await orderService.CompleteOrderAsync(orderId));
                }
            );

            FieldAsync<OrderType>(
                "cancelOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await orderService.CancelOrderAsync(orderId));
                }
            );

            FieldAsync<OrderType>(
                "closeOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await orderService.CloseOrderAsync(orderId));
                }
            );            
        }
    }
}