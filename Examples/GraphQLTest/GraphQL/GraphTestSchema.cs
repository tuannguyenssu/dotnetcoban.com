using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraphQLTest.GraphQL
{
    public class GraphTestSchema : Schema
    {
        public GraphTestSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<GraphTestQuery>();
            Mutation = provider.GetRequiredService<GraphTestMutation>();
        }
    }
}