using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

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