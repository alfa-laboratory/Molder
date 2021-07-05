namespace Molder.Web.Models.Settings
{
    public interface ISettings
    {
        bool IsRemote();
        bool IsBinaryPath();
        bool CheckRemoteRun();
        bool CheckCapability();
    }
}
