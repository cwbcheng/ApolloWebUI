using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ApolloWebUI.Data;

namespace ApolloWebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
                .MigrateDbContext<AppDbContext>((context, services) =>
                {
                    new AppDbContextSeed().SeedAsync(context, services).Wait();
                })
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSystemd()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
    }
}
