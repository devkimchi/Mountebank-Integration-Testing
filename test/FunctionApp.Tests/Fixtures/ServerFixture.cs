using System;
using System.Net;

namespace FunctionApp.Tests.Fixtures
{
    /// <summary>
    /// This represents the fixture entity for external API server.
    /// </summary>
    public abstract class ServerFixture
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ServerFixture"/> class.
        /// </summary>
        /// <param name="env">Mocking environment.</param>
        /// <returns>Returns the new instance of the <see cref="ServerFixture"/> class.</returns>
        public static ServerFixture CreateInstance(string env)
        {
            var type = Type.GetType($"FunctionApp.Tests.Fixtures.{env}ServerFixture");
            var instance = (ServerFixture)Activator.CreateInstance(type);

            return instance;
        }

        /// <summary>
        /// Gets the URL for the health check.
        /// </summary>
        /// <param name="statusCode"><see cref="HttpStatusCode"/> value.</param>
        /// <returns>Returns the URL for the health check.</returns>
        public abstract string GetHealthCheckUrl(HttpStatusCode statusCode = HttpStatusCode.OK);
    }
}