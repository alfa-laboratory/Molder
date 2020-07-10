using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models
{
    public interface IFile
    {
        void Create(string filename, string content = null);
        void Delete();
        string Get();
        bool CheckFileExistence();

    }
}
