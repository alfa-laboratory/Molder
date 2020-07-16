using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models.inerfaces
{
    public interface IWebProvider
    {
        bool Download(string url, string pathToSave, string filename);
    }
}
