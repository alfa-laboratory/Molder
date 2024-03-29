﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Molder.ReportPortal.Infrastructures;
using Molder.ReportPortal.Models;

namespace Molder.ReportPortal.Helpers
{
    public class ConfigOptionsFactory
    {
        public static IOptions<Settings> Create(IConfiguration configuration)
        {
            var blc = configuration.GetSection(Constants.CONFIG_BLOCK);
            var settings = blc.Get<Settings>();
            return Options.Create(settings);
        }
    }
}