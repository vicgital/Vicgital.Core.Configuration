using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Vicgital.Core.Configuration.Helpers;
using Vicgital.Core.Configuration.Services;

namespace Vicgital.Core.Configuration.Extensions
{
    /// <summary>
    /// ServiceCollection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Injects IConfiguration loading the appsettings.{[env].}json and IAppConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppConfigurationService(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSingleton(configuration)
                .AddSingleton<IAppConfigurationService, AppConfigurationService>();        

        #region Private Methods

        

        #endregion





    }
}
