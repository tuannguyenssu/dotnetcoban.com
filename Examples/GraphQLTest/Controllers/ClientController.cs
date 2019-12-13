using GraphQL.Client.Http;
using GraphQL.Common.Request;
using GraphQLTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        public async Task<List<Customer>> Get()
        {
            string graphServerUrl = "http://localhost:5000/graphql";
            using(var client = new GraphQLHttpClient(graphServerUrl))
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
                
                var response = await client.SendQueryAsync(query);
                return response.GetDataFieldAs<List<Customer>>("customers");
            }
        }
    }
}