using Molder.Configuration.Models;
using Molder.Configuration.Exceptions;
using Molder.Controllers;
using Molder.Helpers;
using Molder.Infrastructures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Molder.Configuration.Extension
{
    public static class ConfigExtension
    {
        /// <summary>
        /// Задать значения в контроллере из конфига по тегам
        /// </summary>       
        public static VariableController AddConfig(this VariableController variableController, IOptions<IEnumerable<ConfigFile>> config, IEnumerable<string> tags)
        {
            var controller = variableController;

            var configDictionary = new Dictionary<string, object>();

            tags.ToList().ForEach(tag =>
            {
                config.Value.ToList().ForEach(param =>
                {
                    if(tag == param.Tag)
                    {
                        foreach(var (key, value) in param.Parameters)
                        {
                            try
                            {
                                configDictionary.Add(key, value);
                            }
                            catch(ArgumentException ex)
                            {
                                Log.Logger().LogError($"A value has already been written for the \"{key}\" key. Check the \"{key}\" key in the \"{param.Tag}\" tag");
                                throw new ConfigException($"A value has already been written for the \"{key}\" key. Check the \"{key}\" key in the \"{param.Tag}\" tag. Exception message is: \"{ex.Message}\"");
                            }
                        }
                    }
                    else
                    {
                        Log.Logger().LogDebug($"Tag \"{tag}\" is skipped.");
                    }
                });
            });

            if (!configDictionary.Any()) return controller;
            foreach (var (key, value) in configDictionary)
            {
                controller.SetVariable(key, value.GetType(), value, TypeOfAccess.Global);
            }
            return controller;
        }
    }
}
