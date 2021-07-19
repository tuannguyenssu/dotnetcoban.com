using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FubarDev.FtpServer.AccountManagement;

namespace FtpServerNetCore
{
    public class CustomMembershipProvider : IMembershipProvider
    {
        public Task<MemberValidationResult> ValidateUserAsync(string username, string password)
        {
            if (username == "dotnetcoban" && password == "1234")
            {
                var identity = new ClaimsIdentity();
                return Task.FromResult(
                    new MemberValidationResult(
                        MemberValidationStatus.AuthenticatedUser,
                        new ClaimsPrincipal(identity)));
            }
            return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
        }
    }
}
