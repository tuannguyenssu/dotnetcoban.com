using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreAuthorizationTest
{
    public static class RoleBasedPolicies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";

        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(RoleAdmin)
                .Build();
        }

        public static AuthorizationPolicy IsUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(RoleUser)
                .Build();
        }
    }
}
