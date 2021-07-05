using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;

namespace Molder.Web.Helpers
{
    public static class ConfigOptionsFactory
    {
        public static IOptions<Settings> Create(IConfiguration configuration)
        {
            var blc = configuration.GetSection(Constants.CONFIG_BLOCK).GetSection(Constants.SETTINGS_BLOCK);
            var settings = blc.Get<Settings>();
            return Options.Create(settings);
        }
    }
}