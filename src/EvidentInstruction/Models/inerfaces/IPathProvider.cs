using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models.inerfaces
{
    public interface IPathProvider
    {
        string Combine(string path1, string path2);
    }
}
