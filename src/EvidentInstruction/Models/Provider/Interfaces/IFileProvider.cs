namespace EvidentInstruction.Models.Profider.Interfaces
{
    public interface IFileProvider
    {
        bool CheckFileExtension(string filename);
        bool Exist(string path);
        bool AppendAllText(string filename, string path, string content);
        bool Create(string filename, string path, string content);
        bool WriteAllText(string filename, string path, string content);
        string ReadAllText(string filename, string path);
        bool Delete(string fullpath);
    }
}
