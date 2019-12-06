using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCorePermissionAuthorizationTest.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCorePermissionAuthorizationTest
{
    public static class DatabaseMigrationHelper
    {
        private static async Task EnsureSeedRolesAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(RoleConstants.Admin))
            {
                var role = new IdentityRole { Name = RoleConstants.Admin };

                await roleManager.CreateAsync(role);

                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permissions.Users.Create));
                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permissions.Users.Delete));
            }

            if (!await roleManager.RoleExistsAsync(RoleConstants.User))
            {
                var role = new IdentityRole { Name = RoleConstants.User };

                await roleManager.CreateAsync(role);

                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
            }

        }

        private static async Task EnsureSeedUsersAsync(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            var user = userManager.FindByNameAsync("admin").Result;
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@dotnetcoban.com"
                };
                var result = await userManager.CreateAsync(user, "1234");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await userManager.AddToRoleAsync(user, RoleConstants.Admin);

            }

            user = userManager.FindByNameAsync("user").Result;
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "user",
                    Email = "user@dotnetcoban.com"
                };
                var result = userManager.CreateAsync(user, "1234").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await userManager.AddToRoleAsync(user, RoleConstants.User);
            }
        }

    public static async Task EnsureSeedData(IServiceProvider provider)
        {
            await EnsureSeedRolesAsync(provider);
            await EnsureSeedUsersAsync(provider);
        }
    }
}
