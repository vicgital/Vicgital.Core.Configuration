using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddConfiguration(this IServiceCollection services) =>
            services
                .AddSingleton(GetConfiguration())
                .AddSingleton<IAppConfigurationService, AppConfigurationService>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appConfigEndpoint">Azure App Config endpoint</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationFromAzureAppConfig(this IServiceCollection services, string appConfigEndpoint) =>
            services
                .AddSingleton(GetConfigurationFromAzureAppConfig(appConfigEndpoint))
                .AddSingleton<IAppConfigurationService, AppConfigurationService>();

        public static IServiceCollection AddConfigurationFromAzureAppConfigConnectionString(this IServiceCollection services, string connectionString) =>
            services
                .AddSingleton(GetConfigurationFromAzureAppConfigConnectionString(connectionString))
                .AddSingleton<IAppConfigurationService, AppConfigurationService>();

        #region Private Methods

        private static IConfiguration GetConfigurationFromAzureAppConfigConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("AppConfig connection string must be provided.", nameof(connectionString));

            var builder = GetCommonConfigurationBuilder();

            // Connect to Azure App Configuration using the connection string
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                       .Select(KeyFilter.Any); // Load all keys without labels
            });

            return builder.Build();
        }

        private static IConfiguration GetConfigurationFromAzureAppConfig(string appConfigEndpoint)
        {
            if (string.IsNullOrEmpty(appConfigEndpoint))
                throw new ArgumentException("App Configuration endpoint must be provided.", nameof(appConfigEndpoint));

            var builder = GetCommonConfigurationBuilder();

            // Connect to Azure App Configuration
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
                       .Select(KeyFilter.Any); // Load all keys without labels
            });

            return builder.Build();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = GetCommonConfigurationBuilder();
            return builder.Build();

        }

        private static IConfigurationBuilder GetCommonConfigurationBuilder()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev";

            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            return builder;

        }

        #endregion





    }
}
