using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

namespace Molder.Models.Directory
{
    [ExcludeFromCodeCoverage]
    public abstract class DefaultDirectory : IDirectory
    {
        private AsyncLocal<DirectoryInfo> _directory = new AsyncLocal<DirectoryInfo>();

        public void Create()
        {
            _directory.Value = new DirectoryInfo(Get());
        }

        public bool Exists()
        {
            return _directory.Value.Exists;
        }

        public abstract string Get();

        public IEnumerable<FileInfo> GetFiles(string searchPattern)
        {
            return _directory.Value.GetFiles(searchPattern).ToList();
        }
    }
}
