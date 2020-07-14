using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models
{
    public interface IFile
    {
        void Create(string filename,string path,  string content = null);
        void Delete(string filename, string path);
        string Get();
        bool IsExist(string filename, string path);

    }
}
