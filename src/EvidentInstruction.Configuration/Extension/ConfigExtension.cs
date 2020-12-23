using EvidentInstruction.Configuration.Models;
using EvidentInstruction.Configuration.Exceptions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Helpers;
using EvidentInstruction.Infrastructures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvidentInstruction.Configuration.Extension
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
                        foreach(var p in param.Parameters)
                        {
                            try
                            {
                                configDictionary.Add(p.Key, p.Value);
                            }
                            catch(ArgumentException ex)
                            {
                                Log.Logger().LogError($"A value has already been written for the \"{p.Key}\" key. Check the \"{p.Key}\" key in the \"{param.Tag}\" tag");
                                throw new ConfigException($"A value has already been written for the \"{p.Key}\" key. Check the \"{p.Key}\" key in the \"{param.Tag}\" tag. Exception message is: \"{ex.Message}\"");
                            }
                        }
                    }
                    else
                    {
                        Log.Logger().LogInformation($"Tag \"{tag}\" is skipped.");
                    }
                });
            });

            if(configDictionary.Any())
            {
                foreach (var element in configDictionary)
                {
                    controller.SetVariable(element.Key, element.Value.GetType(), element.Value, TypeOfAccess.Global);
                }
            }
            return controller;
        }
    }
}
