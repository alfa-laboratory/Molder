using System;
using System.Diagnostics.CodeAnalysis;
using Molder.Helpers;
using Microsoft.Extensions.Logging;

namespace Molder.Models.Profider
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
                Log.Logger().LogWarning($"Error while concatenating the string \"{path1}\" and \"{path2}\": \"{e.Message}\"");
                return null;
            }
            catch (ArgumentException e)
            {
                Log.Logger().LogWarning($"Error while concatenating the string \"{path1}\" and \"{path2}\": \"{e.Message}\"");
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
                Log.Logger().LogWarning($"Error getting filename \"{variable}\":\"{e.Message}\"");
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
                Log.Logger().LogWarning($"Error getting filename \"{fullpath}\": \"{e.Message}\"");
                return (null, null);
            }
        }        
    }
}
