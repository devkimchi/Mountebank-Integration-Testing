namespace FunctionApp.Configurations
{
    /// <summary>
    /// This represents the settings entity for endpoints.
    /// </summary>
    public class EndpointsSettings
    {
        /// <summary>
        /// Gets or sets the endpoint for health check.
        /// </summary>
        public virtual string HealthCheck { get; set; }
    }
}
