using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.AdminUiIntegration.Customized
{
    public class AppUser : IdentityExpressUser
    {
        // Add additional profile data for application users by adding properties to this class
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
