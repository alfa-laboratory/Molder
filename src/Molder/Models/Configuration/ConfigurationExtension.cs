using Microsoft.Extensions.Configuration;
using System;

namespace Molder.Models.Configuration
{
    public sealed class ConfigurationExtension
    {
        private static readonly Lazy<ConfigurationExtension> lazy
        = new Lazy<ConfigurationExtension>(() => new ConfigurationExtension());

        public static ConfigurationExtension Instance
            => lazy.Value;

        public IConfiguration Configuration { get; set; } = null;

        private ConfigurationExtension() { }
    }
}