using GraphQL.Types;

namespace GraphQLTest.GraphQL
{
    public class OrderCreateInputType : InputObjectGraphType
    {
        public OrderCreateInputType()
        {
            Name = "OrderCreateInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<IntGraphType>>("customerId");
        }
    }
}