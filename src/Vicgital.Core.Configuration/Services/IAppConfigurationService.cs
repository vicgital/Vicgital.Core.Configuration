using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vicgital.Core.Configuration.Services
{
    public interface IAppConfigurationService
    {
        string GetValue(string key);
        string GetValue(string key, string defaultValue);
    }
}
