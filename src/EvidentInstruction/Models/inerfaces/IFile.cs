using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models
{
    public interface IFile
    {

        string Filename { get; set; }
        string Path { get; set; }
        string Content { get; set; }
        bool Create(string filename, string path, string content = null);
        bool Delete(string filename, string path);
        string Get();
        bool IsExist(string filename, string path);

    }
}
