using Aliencube.AzureFunctions.Extensions.Configuration.AppSettings;
using Aliencube.AzureFunctions.Extensions.Configuration.AppSettings.Extensions;

namespace FunctionApp.Configurations
{
    /// <summary>
    /// This represents the app settings entity.
    /// </summary>
    public class AppSettings : AppSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings()
        {
            this.ExternalApi = this.Config.Get<ExternalApiSettings>("ExternalApi");
        }

        /// <summary>
        /// Gets the <see cref="ExternalApiSettings"/> instance.
        /// </summary>
        public ExternalApiSettings ExternalApi { get; }
    }
}
