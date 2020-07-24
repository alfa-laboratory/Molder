using EvidentInstruction.Controllers;
using EvidentInstruction.Models.Interfaces;
using System;
using EvidentInstruction.Helpers;
using System.IO;
using EvidentInstruction.Config.Infrastructures;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Infrastructures;
using Newtonsoft.Json;
using EvidentInstruction.Config.Models;
using EvidentInstruction.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace EvidentInstruction.Config.Extension
{   
     public static class ConfigExtension
    {
        private static IDirectory BinDirectory = new BinDirectory();
        private static IFile textFile = new TextFile(); 
        private static IPathProvider pathProvider = new PathProvider();

        /// <summary>
        /// Задать значения в контроллере из словаря DictionaryTags
        /// </summary>       
        public static VariableController AddConfig(this VariableController variableController) 
        {            
            var controller = variableController;
            string fullpath = null;

            var path = Environment.GetEnvironmentVariable(DefaultFileName.EXTERNAL_JSON);          

            if (string.IsNullOrWhiteSpace(path))
            {                
                path = BinDirectory.Get();
                fullpath = pathProvider.Combine(path, DefaultFileName.DEFAULT_JSON);
            }

            var filename = pathProvider.GetFileName(fullpath); //переделать

            var dictionary = ConfigHelper.GetDictionary(filename, path); //работает с локальным файлом, но с глобальным нет
                            
            foreach (var element in dictionary)
            {
               controller.SetVariable(element.Key, element.Value.GetType(), element.Value, TypeOfAccess.Global);
            }

            return controller;              
        }
     }
}
