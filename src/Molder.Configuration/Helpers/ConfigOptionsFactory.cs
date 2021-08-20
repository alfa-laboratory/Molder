using Molder.Configuration.Infrastructures;
using Molder.Configuration.Models;
using Molder.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Molder.Configuration.Helpers
{
    public static class ConfigOptionsFactory
    {
        public static IOptions<IEnumerable<ConfigFile>> Create(IConfiguration configuration)
        {
            var config = new List<ConfigFile>();
            var tags = configuration.GetSection(Constants.CONFIG_BLOCK);

            //TODO: если есть секция CONFIG_BLOCK, то все равно падает на Exists

            if (tags.Exists())
            {
                if (tags.GetChildren().Any())
                {
                    config.AddRange(tags.GetChildren().Select(tag => new ConfigFile {Tag = tag.Key, Parameters = tag.Get<Dictionary<string, object>>()}));
                }
                else
                {
                    Log.Logger().LogWarning($"In Section \"{Constants.CONFIG_BLOCK}\" empty blocks (tags) in the appsetting file.");
                }
            }
            else
            {
                Log.Logger().LogWarning($"Section \"{Constants.CONFIG_BLOCK}\" was not found in the appsetting file.");
            }

            return Options.Create(config);
        }
    }
}