using System;
using System.Net;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the Function app service.
    /// </summary>
    public class FunctionAppServerFixture : ServerFixture
    {
        private readonly string _functionAppName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAppServerFixture"/> class.
        /// </summary>
        public FunctionAppServerFixture()
        {
            this._functionAppName = Environment.GetEnvironmentVariable("FunctionAppName");
        }

        /// <inheritdoc />
        public override string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return $"https://{this._functionAppName}.azurewebsites.net/api/ping";
        }
    }
}