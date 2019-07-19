using System;
using System.Net;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the Function app service.
    /// </summary>
    public class FunctionAppServerFixture : ServerFixture
    {
        private const string FunctionAppNameKey = "FunctionAppName";
        private const string FunctionAuthKeyKey = "FunctionAuthKey";

        private readonly string _functionAppName;
        private readonly string _functionAuthKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAppServerFixture"/> class.
        /// </summary>
        public FunctionAppServerFixture()
        {
            this._functionAppName = Environment.GetEnvironmentVariable(FunctionAppNameKey);
            this._functionAuthKey = Environment.GetEnvironmentVariable(FunctionAuthKeyKey);
        }

        /// <inheritdoc />
        public override string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return $"https://{this._functionAppName}.azurewebsites.net/api/ping?code={this._functionAuthKey}";
        }
    }
}