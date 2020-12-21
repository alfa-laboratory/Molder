using EvidentInstruction.Config.Infrastructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EvidentInstruction.Config.Helpers
{
    public static class ConfigOptionsFactory
    {
        public static IOptions<Models.Config> Create(IConfiguration configuration)
        {
            var configConfiguration = new Models.Config();
            configuration.Bind(Constants.CONFIG_BLOCK, configConfiguration);

            return Options.Create(configConfiguration);
        }
    }
}