namespace EvidentInstruction.Models.Assembly.Interfaces
{
    public interface IAssembly
    {
        System.Reflection.Assembly LoadFile(string path);
    }
}
