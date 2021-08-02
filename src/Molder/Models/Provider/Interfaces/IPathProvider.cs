namespace Molder.Models.Profider
{
    public interface IPathProvider
    {
        string? Combine(string path1, string path2);        
        (string?, string) CutFullpath(string path);
        string? GetEnviromentVariable(string varible);
    }
}
