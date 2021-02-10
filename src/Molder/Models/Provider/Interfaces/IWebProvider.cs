namespace Molder.Models.Profider
{
    public interface IWebProvider
    {
        bool Download(string url, string pathToSave, string filename);
    }
}
