using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EvidentInstruction.Helpers;
using EvidentInstruction.Config.Exceptions;

namespace EvidentInstruction.Config.Helpers
{
    public static class DeserializeHelper
    {
        public static T DeserializeObject<T>(string json) //where T:  Models.Config
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T); //возвращает неиницилизированный объект

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch(JsonException e)
            {                
                Log.Logger.Error($"File is empty \"{e.Message}\"");                
                throw new NewconfigExeption($"Deserialize string \"{json}\" failed");                
            }            
        }
    }
}