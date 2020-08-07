using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ApolloWebUI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.CodeAnalysis;
using System.Text;

namespace ApolloWebUI.Data
{
    public class AppDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                if (context.Users.Any() == false)
                {
                    var config = services.GetService<IConfiguration>();
                    var defaultAdmin = config.GetSection("defaultAdmin");
                    _userManager =
                        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var defaultUser = new ApplicationUser
                    {
                        Name = defaultAdmin["name"].ToString(),
                        UserName = defaultAdmin["email"],
                        Email = defaultAdmin["email"],
                    };
                    var result = await _userManager.CreateAsync(defaultUser, defaultAdmin["password"]);

                    if (result.Succeeded == false)
                    {
                        throw new Exception("初始化默认用户失败");
                    }
                }
            }
        }
    }
}
