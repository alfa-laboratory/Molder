using System.Collections.Generic;
using System.IO;

namespace Molder.Models.Directory
{
    public interface IDirectory
    {
        void Create();
        string Get();
        bool Exists();
        IEnumerable<FileInfo> GetFiles(string searchPattern);
    }
}
