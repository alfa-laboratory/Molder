using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Molder.Models.Assembly
{
    [ExcludeFromCodeCoverage]
    public class Assembly : IAssembly
    {
        public System.Reflection.Assembly LoadFile(string path)
        {
            return System.Reflection.Assembly.LoadFile(path);
        }

        public IEnumerable<System.Reflection.Assembly> GetAssembliesInCurrentDomain()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
    }
}