using Aliencube.AzureFunctions.Extensions.DependencyInjection.Abstractions;

using Microsoft.Extensions.Logging;

namespace FunctionApp.Functions
{
    /// <summary>
    /// This provides interfaces to the <see cref="HealthCheckFunction"/> class.
    /// </summary>
    public interface IHealthCheckFunction : IFunction<ILogger>
    {
    }
}