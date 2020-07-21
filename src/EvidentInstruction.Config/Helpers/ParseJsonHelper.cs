using System.Collections.Concurrent;
using EvidentInstruction.Config.Extension;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using EvidentInstruction.Helpers;
using System;
using System.Text;

namespace EvidentInstruction.Config.Helpers
{
    public static class ParseJsonHelper 
    {       
        /// <summary>
        /// Проверка на наличие повторов в словаре DictionaryTags
        /// </summary>       
        private static bool AreDublicates(string key, ConcurrentDictionary<string, object> dic)
        {
            return dic.ContainsKey(key);
        }

        /// <summary>
        /// Получение словаря DictionaryTags
        /// </summary>
        public static ConcurrentDictionary<string, object> GetTagsDictionary()
        {
            ConcurrentDictionary<string, object> dictionaryTags = AddParameters().Item1;
            List<string> dublicatesTagsList = AddParameters().Item2;

            if (dictionaryTags.Count == 0)
                Log.Logger.Warning($"Dictionary with Json parameters are empty");

            if (dublicatesTagsList.Count > 0) 
                GetTagsDublicates(dublicatesTagsList);

            Log.Logger.Information($"Dictionary created. Dictionary has {dictionaryTags.Count} parameters");

            return dictionaryTags;
        }

        /// <summary>
        /// Выбираем список параметров ("parameters")
        /// </summary>       
        private static Models.Parameters[] GetParameter() //jobject 
        {                             
            try
            {
                var objJson = ConfigExtension.ParseJsonFile(); 
                return objJson.Parameters;
            }
            catch (ArgumentException e) 
            {
                Log.Logger.Information($"Keys \"config \" or \"parameters\" not found in Json \"{e.Message}\"");
                return null;            
            }
        }

        /// <summary>
        /// Добавление в словарь DictionaryTags параметров ("key":"value") и 
        /// </summary>        
        private static (ConcurrentDictionary<string, object>, List<string>) AddParameters() 
        {
            var DictionaryTags = new ConcurrentDictionary<string, object>();
            var DublicatesTagsList = new List<string>();

            try
            {
                foreach (var parameters in GetParameter())                
                    foreach (var param in parameters.Param)
                        if (!AreDublicates(param.Key, DictionaryTags))
                            DictionaryTags.TryAdd(param.Key, param.Value);
                        else
                            AddDublicatesTagsInList(param.Key, DublicatesTagsList);
                
            }
            catch (ArgumentNullException e)
            {

                return (null, null);
            }

            return (DictionaryTags, DublicatesTagsList);
        }

        private static void AddDublicatesTagsInList(string tag, List<string> listTag)
        {
            if (!string.IsNullOrWhiteSpace(tag))
                listTag.Add(tag);
        }

        /// <summary>
        /// Вывод список повторных тегов
        /// </summary>        
        private static void GetTagsDublicates(List<string> dublicatesTags)
        {
            var listDublicates = new StringBuilder();

            foreach (var tag in dublicatesTags)
                listDublicates.Append(tag + ',');

            Log.Logger.Information($"Json has {dublicatesTags.Count} dublicates: {listDublicates}");
        }
    }
}
