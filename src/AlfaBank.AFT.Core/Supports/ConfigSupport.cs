using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AlfaBank.AFT.Core.Models.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AlfaBank.AFT.Core.Supports
{
    public class ConfigSupport
    {
        private const string configFile = "aft.config.json";
        private const string environmentConfig = "AFT_CONFIG";

        private readonly ThreadLocal<Config> configLocal;
        public Config Config
        {
            get
            {
                if (configLocal.IsValueCreated)
                {
                    return configLocal.Value;
                }

                return null;
            }
            set => configLocal.Value = value;
        }

        public ConfigSupport()
        {
            configLocal = new ThreadLocal<Config>();
            SetupConfig();
        }

        private void SetupConfig()
        {              
            var configPath = Environment.GetEnvironmentVariable(environmentConfig);

            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + configFile;
            }

            if (File.Exists(configPath))
            {
                var content = File.ReadAllText(configPath);

                Config = TryParse<Config>(content);
                if (Config == null)
                {
                    throw new ArgumentNullException($"Ошибка десериализации файла \"{configPath}\" с содержимым {content}");
                }
            }
            else
            {
                Console.WriteLine($"Конфигурационный файл не задан. Проверьте установку переменной среды \"{environmentConfig}\" или свойство файла \"Copy To Output Directory\".");
            }
        }

        private T TryParse<T>(string content) where T : new()
        {
#pragma warning disable 618
            var generator = new JsonSchemaGenerator();
            var schema = generator.Generate(typeof(T));
            schema = JsonSchema.Parse(schema.ToString().Replace("additionalProperties", "BrokerProperties"));
            var jObject = JObject.Parse(content);
            return jObject.IsValid(schema) ?
                JsonConvert.DeserializeObject<T>(content) : default(T);
#pragma warning restore 618
        }
    }
}