using System;
using System.Diagnostics.CodeAnalysis;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;

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
                Log.Logger.Warning($"Ошибка при объеденении строки \"{path1}\" и \"{path2}\" : \"{e.Message}\" ");
                return null;
            }
            catch (ArgumentException e)
            {
                Log.Logger.Warning($"Ошибка при объеденении строки \"{path1}\" и \"{path2}\" : \"{e.Message}\" ");
                return null;
            }
        }
              
        public string GetFileName(string path)
        {    
            try
            {
                return System.IO.Path.GetFileName(path);
            }
            catch (ArgumentException e)
            {
                Log.Logger.Warning($"Ошибка при получении имени файла \"{path} \" : \"{e.Message}\" ");
                return null;
            }

        }
    }
}
