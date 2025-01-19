using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Vicgital.Core.Configuration.Helpers
{
    public static class ConfigurationHelpers
    {

        public static IConfiguration GetConfiguration()
        {
            var builder = GetCommonConfigurationBuilder();
            return builder.Build();

        }


        public static IConfiguration GetConfigurationFromAzureAppConfigConnectionString(string connectionString)
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

        public static IConfiguration GetConfigurationFromAzureAppConfig(string appConfigEndpoint)
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

        private static IConfigurationBuilder GetCommonConfigurationBuilder()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev";

            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();

            return builder;

        }
    }
}
