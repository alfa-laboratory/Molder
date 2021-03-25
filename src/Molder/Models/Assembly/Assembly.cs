using System.Diagnostics.CodeAnalysis;

namespace Molder.Models.Assembly
{
    [ExcludeFromCodeCoverage]
    public class Assembly : IAssembly
    {
        public System.Reflection.Assembly LoadFile(string path)
        {
            return System.Reflection.Assembly.LoadFile(path);
        }
    }
}
