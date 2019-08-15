using GraphQL.Types;
using GraphQLTest.Models;

namespace GraphQLTest.GraphQL
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(c => c.Address);
        }
    } 
}