using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ASB.Integration.Assessment.WebAPI.DatabaseContext;

namespace ASB.Integration.Assessment.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            using var serviceScope = webHost.Services.CreateScope();

            try
            {
                var creditCardStoreDbContext = serviceScope.ServiceProvider.GetService<CreditCardStoreDbContext>();
                creditCardStoreDbContext.Database.EnsureCreated();
            }
            catch (Exception exception)
            {
                serviceScope.ServiceProvider.GetService<ILogger>()?.LogError(exception, "Error occurred while initializing database");
            }

            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
