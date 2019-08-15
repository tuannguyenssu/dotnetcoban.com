using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQLTest.Models;
using GraphQL.Client;
using GraphQL.Common.Request;

namespace GraphQLTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        public async Task<List<Customer>> Get()
        {
            string graphServerUrl = "http://localhost:5000/graphql";
            using(GraphQLClient client = new GraphQLClient(graphServerUrl))
            {
                var query = new GraphQLRequest
                {
                    Query = @"{
                        customers
                        {
                            name address
                        }
                        }"
                };
                var response = await client.PostAsync(query);
                return response.GetDataFieldAs<List<Customer>>("customers");
            }
        }
    }
}