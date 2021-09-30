using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Molder.ReportPortal.Infrastructures;
using Molder.ReportPortal.Models.Settings;
using System.IO;
using System.Runtime.CompilerServices;

namespace Molder.ReportPortal.Helper
{
    public class ConfigOptionsFactory
    {
        public static IOptions<Settings> Create(IConfiguration configuration)
        {
            var blc = configuration.GetSection(Constants.CONFIG_BLOCK).GetSection(Constants.SETTINGS_BLOCK);
            var settings = blc.Get<Settings>();
            return Options.Create(settings);
        }
    }
}
