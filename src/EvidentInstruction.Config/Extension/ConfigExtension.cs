using EvidentInstruction.Controllers;
using EvidentInstruction.Models.Interfaces;
using EvidentInstruction.Config.Infrastructures;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Infrastructures;
using EvidentInstruction.Config.Models;
using EvidentInstruction.Models;
using System;
using EvidentInstruction.Helpers;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Config.Exceptions;

namespace EvidentInstruction.Config.Extension
{   
     public static class ConfigExtension
    {
        public static  IDirectory BinDirectory = new BinDirectory();
        public static  IFile TextFile = new TextFile();
        public static  IPathProvider PathProvider = new PathProvider();

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
                Log.Logger.Warning($"File \"{fullpath}\" not found.{e.Message}");
                throw new FileIsExistException($"File \"{fullpath}\" not found.");
            }
            catch (FileExistException e)
            {
                Log.Logger.Warning($"File \"{fullpath}\" not found {e.Message}");
                throw new FileIsExistException($"File \"{fullpath}\" not found.");
            }
            catch (ConfigException e)
            {
                Log.Logger.Warning($"Json Exeption. {e.Message}");
                throw new ConfigException($"Json Exeption. {e.Message}");
            }
        }
     }
}
