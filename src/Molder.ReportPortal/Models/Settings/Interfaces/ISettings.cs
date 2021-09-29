namespace Molder.ReportPortal.Models.Settings.Interfaces
{
    public interface ISettings
    {
        bool IsEnabled();
        bool CheckServerSettings();
        bool CheckLaunchSettings();
    }
}
