namespace EvidentInstruction.Models.Interfaces
{
    public interface IWebProvider
    {
        bool Download(string url, string pathToSave, string filename);
    }
}
