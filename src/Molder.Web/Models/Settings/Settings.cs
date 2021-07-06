using Molder.Web.Infrastructures;
using System.Collections.Generic;

namespace Molder.Web.Models.Settings
{
    public class Settings : ISettings
    {
        public BrowserType Browser { get; set; }
        public string BinaryLocation { get; set; }
        public List<string> Options { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }

        private long? _timeout = Constants.DEFAULT_TIMEOUT;
        public long? Timeout {
            get => _timeout * Constants.MS_IN_SEC;
            set => _timeout = value;
        }

        public bool IsRemote { get; set; } = false;

        public Remote Remote { get; set; }

        public bool IsRemoteRun() => Remote != null && IsRemote;
        public bool IsOptions() => Options != null;
        public bool IsBinaryPath() => !string.IsNullOrWhiteSpace(BinaryLocation);

        public bool CheckRemoteRun() => IsRemoteRun() && !string.IsNullOrWhiteSpace(Remote.Url);

        public bool CheckCapability() => Capabilities != null;
    }

    public class Remote
    {
        public string Project { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
    }
}