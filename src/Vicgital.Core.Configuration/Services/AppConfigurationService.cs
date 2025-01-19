using Microsoft.Extensions.Configuration;

namespace Vicgital.Core.Configuration.Services
{
    public class AppConfigurationService(IConfiguration config) : IAppConfigurationService
    {
        private readonly IConfiguration _config = config;

        public string GetValue(string key)
        {
            var value = _config[key] ?? throw new ArgumentException($"{key} was not found in App Configuration");
            return value;
        }

        public string GetValue(string key, string defaultValue)
        {
            var value = _config[key] ?? defaultValue;
            return value;
        }
    }
}
