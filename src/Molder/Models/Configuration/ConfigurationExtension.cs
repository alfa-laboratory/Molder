using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Models.Configuration
{
    [ExcludeFromCodeCoverage]
    public sealed class ConfigurationExtension
    {
        private static readonly Lazy<ConfigurationExtension> lazy
        = new(() => new ConfigurationExtension());

        public static ConfigurationExtension Instance
            => lazy.Value;

        public IConfiguration? Configuration { get; set; } = null;

        private ConfigurationExtension() { }
    }
}