using System;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.DependencyInjection.Abstractions;

using FunctionApp.Configurations;
using FunctionApp.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Functions
{
    /// <summary>
    /// This represents the function entity for health check.
    /// </summary>
    public class HealthCheckFunction : FunctionBase<ILogger>, IHealthCheckFunction
    {
        private readonly ExternalApiSettings _settings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckFunction"/> class.
        /// </summary>
        /// <param name="settings"><see cref="ExternalApiSettings"/> instance.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        public HealthCheckFunction(ExternalApiSettings settings, HttpClient httpClient)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc />
        public override async Task<TOutput> InvokeAsync<TInput, TOutput>(TInput input, FunctionOptionsBase options = null)
        {
            var result = (IActionResult)null;
            var requestUri = $"{this._settings.BaseUri.TrimEnd('/')}/{this._settings.Endpoints.HealthCheck.TrimStart('/')}";
            using (var response = await this._httpClient.GetAsync(requestUri).ConfigureAwait(false))
            {
                try
                {
                    response.EnsureSuccessStatusCode();

                    result = new OkResult();
                }
                catch (Exception ex)
                {
                    var error = new ErrorResponse(ex);

                    result = new ObjectResult(error) { StatusCode = (int)response.StatusCode };
                }
            }

            return (TOutput)result;
        }
    }
}
