using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIEnterpriseCommandCenter.Shared.Constants;
using Microsoft.AspNetCore.Identity;

namespace AIEnterpriseCommandCenter.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
           {
                Roles.Admin,
                Roles.HRManager,
                Roles.ITAdmin,
                Roles.Employee,
                
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
