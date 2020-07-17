using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Models.inerfaces;

namespace EvidentInstruction.Models
{
    [ExcludeFromCodeCoverage]
    public class PathProvider: IPathProvider
    {
        public string Combine(string path1, string path2)
        {
            try
            {
                return System.IO.Path.Combine(path1, path2);
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
