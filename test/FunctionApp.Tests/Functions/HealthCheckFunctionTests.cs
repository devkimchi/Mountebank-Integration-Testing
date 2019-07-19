using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.DependencyInjection.Abstractions;

using FluentAssertions;

using FunctionApp.Configurations;
using FunctionApp.Functions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WorldDomination.Net.Http;

namespace FunctionApp.Tests.Functions
{
    [TestClass]
    public class HealthCheckFunctionTests
    {
        [TestMethod]
        public void Given_Type_It_Should_Inherit_BaseClass()
        {
            typeof(HealthCheckFunction)
                .Should().BeDerivedFrom<FunctionBase<ILogger>>();
        }

        [TestMethod]
        public void Given_Type_It_Should_Implement_Interfaces()
        {
            typeof(HealthCheckFunction)
                .Should().Implement<IHealthCheckFunction>();
        }

        [TestMethod]
        public void Given_Type_When_Instantiated_With_NullParameter_Then_Constructor_Should_Throw_Exception()
        {
            var settings = new Mock<ExternalApiSettings>();
            var httpClient = new HttpClient();

            Action action = () => new HealthCheckFunction(null, httpClient);
            action.Should().Throw<ArgumentNullException>();

            action = () => new HealthCheckFunction(settings.Object, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Parameters_When_Invoked_With_OK_Then_InvokeAsync_Should_Return_Result()
        {
            var endpoints = new Mock<EndpointsSettings>();
            endpoints.SetupGet(p => p.HealthCheck).Returns("ping");

            var settings = new Mock<ExternalApiSettings>();
            settings.SetupGet(p => p.BaseUri).Returns("http://localhost");
            settings.SetupGet(p => p.Endpoints).Returns(endpoints.Object);

            var message = new HttpResponseMessage(HttpStatusCode.OK);
            var options = new HttpMessageOptions()
                              {
                                  HttpMethod = HttpMethod.Get,
                                  HttpResponseMessage = message
                              };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var function = new HealthCheckFunction(settings.Object, httpClient);

            var req = new Mock<HttpRequest>();
            var result = await function.InvokeAsync<HttpRequest, IActionResult>(req.Object)
                                       .ConfigureAwait(false);

            result
                .Should().BeOfType<OkResult>()
                .And.Subject.As<OkResult>()
                    .StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Given_Parameters_When_Invoked_With_Error_Then_InvokeAsync_Should_Return_Result()
        {
            var endpoints = new Mock<EndpointsSettings>();
            endpoints.SetupGet(p => p.HealthCheck).Returns("ping");

            var settings = new Mock<ExternalApiSettings>();
            settings.SetupGet(p => p.BaseUri).Returns("http://localhost");
            settings.SetupGet(p => p.Endpoints).Returns(endpoints.Object);

            var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var options = new HttpMessageOptions()
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = message
            };
            var handler = new FakeHttpMessageHandler(options);
            var httpClient = new HttpClient(handler);
            var function = new HealthCheckFunction(settings.Object, httpClient);

            var req = new Mock<HttpRequest>();
            var result = await function.InvokeAsync<HttpRequest, IActionResult>(req.Object)
                                       .ConfigureAwait(false);

            result
                .Should().BeOfType<ObjectResult>()
                .And.Subject.As<ObjectResult>()
                    .StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
