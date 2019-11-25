using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Data
{
    public static class WebHostMigrationExtensions
    {
        public static IHost MigrateDbContext<TContext>(
            this IHost host, 
            Action<TContext, IServiceProvider> seed)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    context.Database.Migrate();
                    seed(context, services);
                    logger.LogInformation($"执行DbContext{typeof(TContext).Name} seed 方法成功");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"执行DbContext{typeof(TContext).Name} seed 方法失败");
                }
            }

            return host;
        }
    }
}
