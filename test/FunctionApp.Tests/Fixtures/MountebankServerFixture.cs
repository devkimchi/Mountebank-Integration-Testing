using System.Net;

using MbDotNet;
using MbDotNet.Enums;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for the Mountebank server.
    /// </summary>
    public class MountebankServerFixture : ServerFixture
    {
        private readonly MountebankClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MountebankServerFixture"/> class.
        /// </summary>
        public MountebankServerFixture()
        {
            this._client = new MountebankClient();
        }

        /// <inheritdoc />
        public override string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this._client.DeleteImposter(8080);
            var imposter = this._client.CreateHttpImposter(8080, "OK");

            imposter.AddStub().OnPathAndMethodEqual("/ping", Method.Get).ReturnsStatus(statusCode);
            this._client.Submit(imposter);

            return "http://localhost:7071/ping";
        }
    }
}