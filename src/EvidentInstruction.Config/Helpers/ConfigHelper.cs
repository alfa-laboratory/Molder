using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models;
using EvidentInstruction.Models.Interfaces;
using EvidentInstruction.Config.Exceptions;
using EvidentInstruction.Exceptions;

namespace EvidentInstruction.Config.Helpers
{
    public static class ConfigHelper 
    {       
        public static IFile File = new TextFile();

        /// <summary>
        /// Получение словаря dictionaryTags и списка повторных параметров dublicatesTagsList и проверить dublicatesTagsList
        /// </summary>
        public static ConcurrentDictionary<string, object> GetTagsDictionary(Models.Config config) 
        {
            var (tags, dublicatesTags) = AddParameters(config);
            
                if (dublicatesTags.Any())
                {
                    Log.Logger.Warning($"Json has {dublicatesTags.Count} dublicates:" + System.Environment.NewLine + $"{Message.CreateMessage(dublicatesTags)}");                    
                    throw new ConfigException($"Json has {dublicatesTags.Count} dublicates:" + System.Environment.NewLine + $"{Message.CreateMessage(dublicatesTags)}");
                }
                else
                {
                    Log.Logger.Information($"Dublicates in config file not found");
                }
                if (tags.Any())
                {
                    Log.Logger.Information($"Dictionary created. Dictionary has {tags.Count} parameters");
                }
                else
                {
                    Log.Logger.Warning("Dictionary not created");
                }            

            return tags;
        }
        /// <summary>
        /// Получить словарь с тегами
        /// </summary>        
        public static ConcurrentDictionary<string, object> GetDictionary(string filename, string path)
        {
            try
            {
                var content = File.GetContent(filename, path);

                if (string.IsNullOrWhiteSpace(content))
                {
                    Log.Logger.Warning($"File \"{filename}\" is empty");
                    return new ConcurrentDictionary<string, object>();
                }

                var config = DeserializeHelper.DeserializeObject<Models.Config>(content);

                if (config == null)
                {
                    Log.Logger.Warning("Json model is empty for config file");
                    return new ConcurrentDictionary<string, object>();
                }

                return GetTagsDictionary(config);
            }
            catch(FileExistException e)
            {
                Log.Logger.Warning($"Config file \"{filename}\" not found: {e.Message}"); 
                throw new FileExistException($"Config file \"{filename}\" not found in path \"{path}\"");
            }
            catch (NoFileNameException e)
            {
                Log.Logger.Warning($"Config filename \"{filename}\" is empty: {e.Message}");
                throw new NoFileNameException($"Config filename is empty");
            }
        }

        /// <summary>
        /// Добавление в словарь DictionaryTags параметров ("key":"value") и дублей в dublicatesTagsList
        /// </summary>        
        public static (ConcurrentDictionary<string, object>, List<string>) AddParameters(Models.Config config) 
        {
            var tags = new ConcurrentDictionary<string, object>();
            var dublicatesTags = new List<string>();
            
             if (config.Parameters == null)
             {
                 Log.Logger.Information($"Keys \"config \" or \"parameters\" not found in Json ");
                 return (tags, dublicatesTags);
             } 

            foreach (var parameter in config.Parameters)
            {    
                foreach (var param in parameter.Param)
                {      
                    if (tags.ContainsKey(param.Key))
                        dublicatesTags.Add(param.Key);
                    else
                        tags.TryAdd(param.Key, param.Value);
                }
            }       

            return (tags, dublicatesTags);
        }
    }
}
