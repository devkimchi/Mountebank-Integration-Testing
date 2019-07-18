using System.Net.Http;

using FunctionApp.Configurations;
using FunctionApp.Functions;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp.StartUp))]
namespace FunctionApp
{
    /// <summary>
    /// This represents the IoC container entity.
    /// </summary>
    public class StartUp : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureHttpClient(builder.Services);
            this.ConfigureAppSettings(builder.Services);
            this.ConfigureFunctions(builder.Services);
        }

        private void ConfigureHttpClient(IServiceCollection services)
        {
            var httpClient = new HttpClient();
            services.AddSingleton(httpClient);
            //services.AddHttpClient<IHealthCheckFunction, HealthCheckFunction>();
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();
            services.AddSingleton(p => p.GetService<AppSettings>().ExternalApi);
        }

        private void ConfigureFunctions(IServiceCollection services)
        {
            services.AddTransient<IHealthCheckFunction, HealthCheckFunction>();
        }
    }
}
