using EvidentInstruction.Models.Assembly.Interfaces;

namespace EvidentInstruction.Models.Assembly
{
    public class Assembly : IAssembly
    {
        public System.Reflection.Assembly LoadFile(string path)
        {
            return System.Reflection.Assembly.LoadFile(path);
        }
    }
}
