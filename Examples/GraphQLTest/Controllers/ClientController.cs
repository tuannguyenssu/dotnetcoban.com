using GraphQL;
using GraphQL.Client.Http;
using GraphQLTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Client.Serializer.SystemTextJson;

namespace GraphQLTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        public async Task<List<Customer>> Get()
        {
            string graphServerUrl = "http://localhost:5000/graphql";
            using(var client = new GraphQLHttpClient(graphServerUrl, new SystemTextJsonSerializer()))
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
                
                var response = await client.SendQueryAsync<List<Customer>>(query);
                return response.Data;
                // response.GetDataFieldAs<List<Customer>>("customers");
            }
        }
    }
}