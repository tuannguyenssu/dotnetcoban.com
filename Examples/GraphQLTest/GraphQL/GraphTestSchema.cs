using GraphQL;
using GraphQL.Types;

namespace GraphQLTest.GraphQL
{
    public class GraphTestSchema : Schema
    {
        public GraphTestSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<GraphTestQuery>();
            Mutation = resolver.Resolve<GraphTestMutation>();
        }
    }
}