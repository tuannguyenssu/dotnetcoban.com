using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;

namespace IdentityServer.Customized
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public IdentityClaimsProfileService(UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);
            //var principal = await _claimsFactory.CreateAsync(user);

            //var claims = principal.Claims.ToList();
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            //claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            //claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            // note: to dynamically add roles (ie. for users other than consumers - simply look them up by sub id
            //claims.Add(new Claim(ClaimTypes.Role, "consumer")); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4
            //claims.Add(new Claim(JwtClaimTypes.Role, "admin")); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4

            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, user.FullName));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, user.Role));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
