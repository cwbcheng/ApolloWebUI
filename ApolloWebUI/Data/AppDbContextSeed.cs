using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ApolloWebUI.Data
{
    public class AppDbContextSeed
    {
        private UserManager<IdentityUser> _userManager;
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                if (context.Users.Any() == false)
                {
                    _userManager =
                        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var defaultUser = new IdentityUser
                    {
                        UserName = "a@b.c",
                        Email = "a@b.c",
                    };
                    var result = await _userManager.CreateAsync(defaultUser, "123456");

                    if (result.Succeeded == false)
                    {
                        throw new Exception("初始化默认用户失败");
                    }
                }
            }
        }
    }
}
