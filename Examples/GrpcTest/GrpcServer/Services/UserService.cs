using Grpc.Core;
using GrpcUserEndpoint;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static GrpcUserEndpoint.GrpcUser;

namespace GrpcServer
{
    public class UserService : GrpcUserBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Received request with Id : {request.Id}");
            return Task.FromResult(new GetUserResponse
            {
                Id = request.Id,
                Name = Guid.NewGuid().ToString()
            });
        }
    }
}
