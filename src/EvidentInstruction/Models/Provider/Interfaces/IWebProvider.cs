namespace EvidentInstruction.Models.Profider.Interfaces
{
    public interface IWebProvider
    {
        bool Download(string url, string pathToSave, string filename);
    }
}
