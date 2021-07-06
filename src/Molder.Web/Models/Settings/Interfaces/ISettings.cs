namespace Molder.Web.Models.Settings
{
    public interface ISettings
    {
        bool IsRemoteRun();
        bool IsBinaryPath();
        bool CheckRemoteRun();
        bool IsOptions();
        bool CheckCapability();
    }
}
