using System;
using System.Net;
using System.Threading.Tasks;

using FunctionApp.Functions;
using FunctionApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    /// <summary>
    /// This represents the HTTP trigger entity for health check.
    /// </summary>
    public class HealthCheckHttpTrigger
    {
        private readonly IHealthCheckFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckHttpTrigger"/> class.
        /// </summary>
        /// <param name="function"></param>
        public HealthCheckHttpTrigger(IHealthCheckFunction function)
        {
            this._function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <summary>
        /// Invokes a ping request.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName(nameof(HealthCheckHttpTrigger.PingAsync))]
        public async Task<IActionResult> PingAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ping")] HttpRequest req,
            ILogger log)
        {
            this._function.Log = log;

            var result = (IActionResult)null;
            try
            {
                result = await this._function
                                   .InvokeAsync<HttpRequest, IActionResult>(req)
                                   .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = new ErrorResponse(ex);

                result = new ObjectResult(error) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

            return result;
        }
    }
}