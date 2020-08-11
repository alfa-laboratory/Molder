using EvidentInstruction.Controllers;
using EvidentInstruction.Config.Infrastructures;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Infrastructures;
using EvidentInstruction.Models;
using EvidentInstruction.Helpers;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Config.Exceptions;
using System;
using EvidentInstruction.Models.Profider.Interfaces;
using EvidentInstruction.Models.Directory.Interfaces;
using EvidentInstruction.Config.Models.Directory;

namespace EvidentInstruction.Config.Extension
{   
     public static class ConfigExtension
    {
        [ThreadStatic] public static IDirectory BinDirectory = new BinDirectory();
        [ThreadStatic] public static IPathProvider PathProvider = new PathProvider();

        /// <summary>
        /// Задать значения в контроллере из словаря DictionaryTags
        /// </summary>       
        public static VariableController AddConfig(this VariableController variableController) 
        {   
           var controller = variableController;

           var filename = DefaultFileName.DEFAULT_JSON;
           var path = BinDirectory.Get(); 
            
           var fullpath = PathProvider.GetEnviromentVariable(DefaultFileName.EXTERNAL_JSON);

            try
            {

                if (!string.IsNullOrWhiteSpace(fullpath))
                {
                    (path, filename) = PathProvider.CutFullpath(fullpath);
                }
                var dictionary = ConfigHelper.GetDictionary(filename, path);

                foreach (var element in dictionary)
                {
                    controller.SetVariable(element.Key, element.Value.GetType(), element.Value, TypeOfAccess.Global);
                }

                return controller;
            }
            catch(NoFileNameException e)
            {               
                Log.Logger.Warning($"Config filename \"{filename}\" is empty: {e.Message}");
                throw new NoFileNameException($"Config filename is empty");
            }
            catch (FileExistException e)
            {                
                Log.Logger.Warning($"Config file \"{filename}\" not found: {e.Message}");
                throw new FileExistException($"Config file \"{filename}\" not found in path \"{path}\"");
            }
            catch (ConfigException e)
            {
                Log.Logger.Warning($"Json Exception in config file: {e.Message}");
                throw new DublicateTagsException($"Json Exception in config file: {e.Message}");
            }
        }
     }
}
