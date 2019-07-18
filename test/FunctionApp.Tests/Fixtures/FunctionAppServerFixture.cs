using System.Net;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the Function app service.
    /// </summary>
    public class FunctionAppServerFixture : ServerFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAppServerFixture"/> class.
        /// </summary>
        public FunctionAppServerFixture()
        {
        }

        /// <inheritdoc />
        public override string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return "https://my-function-app.azurewebsites.net/ping";
        }
    }
}