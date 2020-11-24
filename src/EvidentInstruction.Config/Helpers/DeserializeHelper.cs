using Newtonsoft.Json;
using EvidentInstruction.Helpers;
using EvidentInstruction.Config.Exceptions;
using Microsoft.Extensions.Logging;

namespace EvidentInstruction.Config.Helpers
{
    public static class DeserializeHelper
    {
        public static T DeserializeObject<T>(string json) 
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T); 

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch(JsonException e)
            {
                Log.Logger().LogError($"DeserializeObject is error, because file is empty: \"{e.Message}\"");                
                throw new DeserializeException($"Deserialize string \"{json}\" failed", e);                
            }            
        }
    }
}