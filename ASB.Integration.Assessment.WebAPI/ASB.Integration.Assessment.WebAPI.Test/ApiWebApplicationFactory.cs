using ASB.Integration.Assessment.WebAPI.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASB.Integration.Assessment.WebAPI.Test
{
    /// <summary>
    /// API Web application.
    /// </summary>
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                  .Build();

                config.AddConfiguration(integrationConfig);
            });

            // is called after the `ConfigureServices` from the Startup
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IAuthenticationService, AuthenticationService>();
            });
        }
    }
}
