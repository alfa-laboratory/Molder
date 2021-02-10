namespace Molder.Models.Assembly
{
    public interface IAssembly
    {
        System.Reflection.Assembly LoadFile(string path);
    }
}
