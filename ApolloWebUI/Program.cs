using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ApolloWebUI.Data;
using ApolloWebUI.Model;

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
                .MigrateDbContext<CallChainHandleDbContext>((context, services) =>
                {

                })
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSystemd()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://0.0.0.0:5000")
                    .UseStartup<Startup>();
                });
    }
}
