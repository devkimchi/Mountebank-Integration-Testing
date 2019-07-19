using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.DependencyInjection.Abstractions;

using FluentAssertions;

using FunctionApp.Functions;
using FunctionApp.Tests.Fixtures;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace FunctionApp.Tests
{
    [TestClass]
    public class HealthCheckHttpTriggerTests
    {
        private const string CategoryE2E = "E2E";
        private const string DefaultServerName = "Mountebank";
        private const string ServerNameKey = "ServerName";

        private ServerFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            var serverName = Environment.GetEnvironmentVariable(ServerNameKey);
            if (string.IsNullOrWhiteSpace(serverName))
            {
                serverName = DefaultServerName;
            }

            this._fixture = ServerFixture.CreateInstance(serverName);
        }

        [TestMethod]
        public void Given_Type_It_Should_Declare_Methods()
        {
            var method = typeof(HealthCheckHttpTrigger)
                             .Should().HaveMethod(nameof(HealthCheckHttpTrigger.PingAsync), new[] { typeof(HttpRequest), typeof(ILogger) })
                                 .Which.Should().Return<Task<IActionResult>>()
                                 .And.Subject;

            method.GetCustomAttributes(typeof(FunctionNameAttribute), false)
                .Should().ContainSingle()
                .And.Subject.Single().As<FunctionNameAttribute>()
                    .Name.Should().Be(nameof(HealthCheckHttpTrigger.PingAsync));

            var attribute = method.GetParameters().First().GetCustomAttributes(typeof(HttpTriggerAttribute), false)
                                  .Should().ContainSingle()
                                  .And.Subject.Single().As<HttpTriggerAttribute>();

            attribute.AuthLevel.Should().Be(AuthorizationLevel.Function);
            attribute.Methods.Should().ContainSingle()
                .And.Subject.Single().Should().BeEquivalentTo("get");
            attribute.Route.Should().BeEquivalentTo("ping");
        }

        [TestMethod]
        public void Given_NullParameter_When_Instantiated_Then_Constructor_Should_Throw_Exception()
        {
            Action action = () => new HealthCheckHttpTrigger(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Parameters_When_Invoked_Then_InvokeAsync_Should_Return_Result()
        {
            var function = new Mock<IHealthCheckFunction>();

            var result = new OkResult();
            function.Setup(p => p.InvokeAsync<HttpRequest, IActionResult>(It.IsAny<HttpRequest>(), It.IsAny<FunctionOptionsBase>()))
                    .ReturnsAsync(result);

            var trigger = new HealthCheckHttpTrigger(function.Object);

            var req = new Mock<HttpRequest>();
            var log = new Mock<ILogger>();

            var response = await trigger.PingAsync(req.Object, log.Object).ConfigureAwait(false);

            response
                .Should().BeOfType<OkResult>()
                .And.Subject.As<OkResult>()
                    .StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        [TestCategory(CategoryE2E)]
        public async Task Given_Url_When_Invoked_Then_Trigger_Should_Return_Healthy()
        {
            var uri = this._fixture.GetHealthCheckUrl();

            using (var http = new HttpClient())
            using (var res = await http.GetAsync(uri))
            {
                res.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task Given_Url_When_Invoked_Then_Trigger_Should_Return_Unhealthy()
        {
            var uri = this._fixture.GetHealthCheckUrl(HttpStatusCode.InternalServerError);

            using (var http = new HttpClient())
            using (var res = await http.GetAsync(uri))
            {
                res.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
        }
    }
}