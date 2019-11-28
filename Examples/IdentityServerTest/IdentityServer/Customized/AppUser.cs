using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Customized
{
    public class AppUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
