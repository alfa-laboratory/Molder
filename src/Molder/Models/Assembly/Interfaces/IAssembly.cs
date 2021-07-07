using System.Collections.Generic;

namespace Molder.Models.Assembly
{
    public interface IAssembly
    {
        System.Reflection.Assembly LoadFile(string path);
        IEnumerable<System.Reflection.Assembly> GetAssembliesInCurrentDomain();
        
    }
}