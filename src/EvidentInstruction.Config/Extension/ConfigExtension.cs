using EvidentInstruction.Controllers;
using Newtonsoft.Json.Linq;
using System;
using EvidentInstruction.Helpers;
using System.IO;
using EvidentInstruction.Config.Infrastructures;
using EvidentInstruction.Config.Helpers;
using EvidentInstruction.Infrastructures;
using Newtonsoft.Json;

namespace EvidentInstruction.Config.Extension
{   
     public static class ConfigExtension 
     {       
        public static VariableController AddConfig(this VariableController variableController) 
        {
            var controller = variableController;

            foreach (var param in ParseJsonHelper.GetTagsDictionary()) //try catch
                controller.SetVariable(param.Key, typeof(string), param.Value, TypeOfAccess.Global);

            return controller;
        }

        private static string GetFilePath()
        {
            var path = Environment.CurrentDirectory;  // bin\\Debug\\netstandard2.0"
            string fileName = null;

                if (DefaultFileName.EXTERNAL_JSOM.Contains(".json")) 
                {
                    fileName = DefaultFileName.EXTERNAL_JSOM;
                    Log.Logger.Information($"Get file \"{path}\" is't Json");
                }
                else
                {
                    fileName = DefaultFileName.DEFAULT_JSON;
                    Log.Logger.Information($"Get local file \"{path}{Path.DirectorySeparatorChar}{fileName}\" ");
                }

            return $"{path}{Path.DirectorySeparatorChar}{fileName}"; 
        }

        private static string IsFileExists() //тест на то, что название файлов совпадают
        {
            var path = GetFilePath();
            if (File.Exists(path))
            {
                Log.Logger.Information($"Get file \"{path}\" ");
                return path;
            }                        
            else
            {
                Log.Logger.Error($"File:\"{path}\" not found");
                throw new ArgumentException($"File with path:{path} not found");                
            }  
        }      

        private static string ReadJsonFile() //path входной 
        {
            string file = null;

            try
            {
                file = File.ReadAllText(IsFileExists()); //из другой библиотеки
                Log.Logger.Information($"File \" {GetFilePath()} \" has been read");
            }
            catch(FileNotFoundException e)
            {
                Log.Logger.Information($"File \"{GetFilePath()}\" not found. Exception: \" {e.Message}\"");
                return null;
            }

            return file;    
        }

        public static Models.Config ParseJsonFile() 
        {
            var jsonString = ReadJsonFile();
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonString)) 
                {
                    var JsonObject = JsonConvert.DeserializeObject<Models.Config>(jsonString);
                   //проверку что не пустой
                    return JsonObject;
                }
            }
            catch
            {
                Log.Logger.Error($"File \"{GetFilePath()}\" is Empty"); 
                throw new ArgumentNullException($"File \"{GetFilePath()}\" is Empty");             
            }

            return null;
        }       
    }
}
