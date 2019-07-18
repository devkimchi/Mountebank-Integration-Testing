namespace FunctionApp.Configurations
{
    /// <summary>
    /// This represents the settings entity for external API.
    /// </summary>
    public class ExternalApiSettings
    {
        /// <summary>
        /// Gets or sets the base URI.
        /// </summary>
        public virtual string BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EndpointsSettings"/> instance.
        /// </summary>
        public virtual EndpointsSettings Endpoints { get; set; }
    }
}
