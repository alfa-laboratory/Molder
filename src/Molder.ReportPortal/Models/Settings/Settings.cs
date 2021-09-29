using Molder.ReportPortal.Models.Settings.Interfaces;
using System;
using System.Collections.Generic;

namespace Molder.ReportPortal.Models.Settings
{
    public class Settings : ISettings
    {
        public bool Enabled { get; set; } = false;
        public LaunchSettings LaunchSettings { get; set; }
        public ServerSettings ServerSettings { get; set; }

        public bool IsEnabled() => Enabled;
        public bool CheckServerSettings() => !String.IsNullOrEmpty(ServerSettings.Project) ||
            !String.IsNullOrEmpty(ServerSettings.Url) ||
            !String.IsNullOrEmpty(ServerSettings.Token);
        public bool CheckLaunchSettings() => !String.IsNullOrEmpty(LaunchSettings.Name);
    }

    public class LaunchSettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRerun { get; set; } = false;
        public string RerunOfLaunchUuid { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public List<string> Tags { get; set; }
    }

    public class ServerSettings
    {
        public string Project { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
    }
}
