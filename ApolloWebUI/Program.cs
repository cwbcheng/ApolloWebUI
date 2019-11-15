using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ApolloWebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://localhost:8000;https://localhost:8001")
                    .UseStartup<Startup>();
                });
    }
}
