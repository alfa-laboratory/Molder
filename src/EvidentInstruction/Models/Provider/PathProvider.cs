using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Profider.Interfaces;

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
                Log.Logger.Warning($"Error while concatenating the string \"{path1}\" and \"{path2}\": \"{e.Message}\"");
                return null;
            }
            catch (ArgumentException e)
            {
                Log.Logger.Warning($"Error while concatenating the string \"{path1}\" and \"{path2}\": \"{e.Message}\"");
                return null;
            }
        }

        public string GetEnviromentVariable(string variable)
        {
            try
            {
                return Environment.GetEnvironmentVariable(variable);
            }
            catch (ArgumentException e)
            {
                Log.Logger.Warning($"Error getting filename \"{variable}\":\"{e.Message}\"");
                return null;
            }
        }

        public (string,string) CutFullpath(string fullpath)
        {    
            try
            {
                return (System.IO.Path.GetDirectoryName(fullpath), System.IO.Path.GetFileName(fullpath));                
            }
            catch (ArgumentException e)
            {
                Log.Logger.Warning($"Error getting filename \"{fullpath}\": \"{e.Message}\"");
                return (null, null);
            }
        }        
    }
}
