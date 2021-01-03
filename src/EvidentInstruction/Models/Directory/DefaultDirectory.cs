using EvidentInstruction.Models.Directory.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EvidentInstruction.Models.Directory
{
    public abstract class DefaultDirectory : IDirectory
    {
        [ThreadStatic]
        private DirectoryInfo _directory = null;

        public void Create()
        {
            _directory = new DirectoryInfo(Get());
        }

        public bool Exists()
        {
            return _directory.Exists;
        }

        public abstract string Get();

        public IEnumerable<FileInfo> GetFiles(string searchPattern)
        {
            return _directory.GetFiles(searchPattern).ToList();
        }
    }
}
