using System.Net;

using MbDotNet;
using MbDotNet.Enums;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the localhost server.
    /// </summary>
    public class LocalhostServerFixture : ServerFixture
    {
        private readonly MountebankClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalhostServerFixture"/> class.
        /// </summary>
        public LocalhostServerFixture()
        {
            this._client = new MountebankClient();
        }

        /// <inheritdoc />
        public override string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this._client.DeleteImposter(8080);
            var imposter = this._client.CreateHttpImposter(8080, statusCode.ToString());

            imposter.AddStub().OnPathAndMethodEqual("/api/ping", Method.Get).ReturnsStatus(statusCode);
            this._client.Submit(imposter);

            return "http://localhost:7071/api/ping";
        }
    }
}