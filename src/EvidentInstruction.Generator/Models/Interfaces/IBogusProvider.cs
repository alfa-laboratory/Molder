using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Generator.Models
{
    interface IBogusProvider
    {
        string Guid();
        DateTime Between(DateTime? start = null, DateTime? end = null);
        string Numbers(int len);
        string Phone(string format);
        string String(int len);
        string Chars(int len);
    }
}
