using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static async Task Main()
        {
            var httpClient = new HttpClient();
            var identityTokenUrl = "http://localhost:5000/connect/token";
            var resourceUrl = "http://localhost:6000/api/values";

            //Just a sample call with an invalid access token.
            // The expected response from this call is 401 Unauthorized
            var apiResponse = await httpClient.GetAsync(resourceUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_access_token");

            //The API is protected, let's ask the user for credentials and exchanged them with an access token
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                //Ask User
                Console.Write("Username:");
                var username = Console.ReadLine();
                Console.Write("Password:");
                var password = Console.ReadLine();

                //Make the call and get the access token back
                var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = identityTokenUrl,
                    GrantType = "password",

                    ClientId = "ConsoleApp_ClientId",
                    ClientSecret = "secret_for_the_consoleapp",
                    Scope = "ApiName",

                    UserName = username,
                    Password = password.ToSha256()
                });

                //all good?
                if (!identityServerResponse.IsError)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("SUCCESS!!");
                    Console.WriteLine();
                    Console.WriteLine("Access Token: ");
                    Console.WriteLine(identityServerResponse.AccessToken);

                    //Call the API with the correct access token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);
                    apiResponse = await httpClient.GetAsync(resourceUrl);

                    Console.WriteLine();
                    Console.WriteLine("API response:");
                    Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Failed to login with error:");
                    Console.WriteLine(identityServerResponse.ErrorDescription);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("YOU ARE NOT PROTECTED!!!");
            }

            Console.ReadKey();
        }
    }
}