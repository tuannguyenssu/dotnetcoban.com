using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreAuthorizationTest
{
    public static class PolicyBasedPolicies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";

        public const string ClaimDiploma = "Diploma";
        public const string ClaimCmnd = "CMND";

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimDiploma)
                .RequireClaim(ClaimCmnd)
                .Build();
        }

        public static AuthorizationPolicy IsUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimCmnd)
                .Build();
        }
    }
}