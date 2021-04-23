using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Molder.Helpers;
using Molder.Models;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Molder.Infrastructures;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Molder.Controllers
{
    public class VariableController
    {
        private Lazy<ConcurrentDictionary<string, Variable>> _variables = new Lazy<ConcurrentDictionary<string, Variable>>(() => new ConcurrentDictionary<string, Variable>());

        public ConcurrentDictionary<string, Variable> Variables
        {
            get => _variables.Value;
        } 

        public string GetVariableName(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                var varName = key;
                if (varName.IndexOf('.') > 0)
                {
                    varName = varName.Substring(0, varName.IndexOf('.'));
                }

                if (varName.IndexOf('[') > 0)
                {
                    varName = varName.Substring(0, varName.IndexOf('['));
                }

                return varName;
            }
            else
            {
                return null;
            }
        }
        public Variable GetVariable(string key)
        {
            var correcKey = GetVariableName(key);
            if (correcKey == null) return null;

            if (Variables.ContainsKey(correcKey))
            {
                if (Variables[correcKey].TypeOfAccess == TypeOfAccess.Global || Variables[correcKey].TypeOfAccess == TypeOfAccess.Default)
                {
                    Log.Logger().LogInformation($"Element with key: \"{key}\" contains value {Variables[correcKey].Value} with type '{Variables[correcKey].TypeOfAccess}'");
                }
            }

            return Variables.SingleOrDefault(_ => _.Key == GetVariableName(key)).Value;            
        }
        public bool CheckVariableByKey(string key)
        {
            return Variables.Any(_ => _.Key == GetVariableName(key));
        }
        public void SetVariable(string key, Type type, object value, TypeOfAccess accessType = TypeOfAccess.Local)
        {
            var varName = GetVariableName(key);

            if(string.IsNullOrWhiteSpace(varName))
            {
                Log.Logger().LogInformation($"Key: \"{key}\" is empty");
                return;
            }
            
            if (Variables.ContainsKey(varName))
            {
                switch(Variables[varName].TypeOfAccess)
                {
                    case TypeOfAccess.Global:
                        {
                            switch(accessType)
                            {
                                case TypeOfAccess.Local:
                                    {
                                        Log.Logger().LogInformation($"Element with key: \"{key}\" and '{Variables[varName].TypeOfAccess}' type has been replaced with type '{accessType}'");
                                        break;
                                    }
                                case TypeOfAccess.Default:
                                    {
                                        Log.Logger().LogInformation($"Element with key: \"{key}\" has already created  with type '{Variables[varName].TypeOfAccess}'");
                                        return;
                                    }
                                case TypeOfAccess.Global:
                                    {
                                        Log.Logger().LogWarning($"Element with key: \"{key}\" has already created with type 'Global'");
                                        throw new ArgumentException($"Element with key: \"{key}\" has already created with type 'Global'");
                                    }
                            }
                            break;
                        }
                    case TypeOfAccess.Default:
                        {
                            switch (accessType)
                            {
                                case TypeOfAccess.Local:
                                    {
                                        Log.Logger().LogInformation($"Element with key: \"{key}\" and '{Variables[varName].TypeOfAccess}' type has been replaced with type '{accessType}'");
                                        break;
                                    }
                            }
                            break;
                        }
                }

            }

            var variable = new Variable() { Type = type, Value = value, TypeOfAccess = accessType };
            Variables.AddOrUpdate(varName, variable, (k, v) => variable);
        } 
        public object GetVariableValue(string key)
        {
            try
            {
                var name = key;
                var keyPath = string.Empty;
                var index = -1;
                string path = null;
                if (key.IndexOf('.') > 0)
                {
                    name = key.Substring(0, key.IndexOf('.'));
                    path = key.Substring(key.IndexOf('.') + 1);
                }

                if (name.IndexOf('[') > 0 && name.IndexOf(']') > name.IndexOf('['))
                {
                    if (!int.TryParse(key.Substring(name.IndexOf('[') + 1, Math.Max(0, name.IndexOf(']') - name.IndexOf('[') - 1)), out index))
                    {
                        index = -1;
                    }

                    name = key.Split('[').First();

                    keyPath = Regex.Match(key ?? string.Empty, StringPattern.BRACES, RegexOptions.None).Groups[1].Value;
                }

                var var = Variables.SingleOrDefault(_ => _.Key == name).Value;
                if (var == null)
                {
                    return null;
                }

                var varValue = var.Value;
                var varType = var.Type;

                if (varValue == null)
                {
                    return varType.GetDefault();
                }

                if (varType.HasElementType && index >= 0)
                {
                    var objArray = ((Array)varValue).Cast<object>().ToArray();
                    varValue = (objArray)[index];
                    varType = varType.GetElementType();
                }

                if (typeof(BsonDocument).IsAssignableFrom(varType))
                {
                    var json = JObject.Parse(((BsonDocument)varValue).ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.CanonicalExtendedJson }));
                    return json.SelectToken(path?.Remove(0, 2) ?? "/*") ?? varValue;
                }

                if (typeof(JObject).IsAssignableFrom(varType))
                {
                    var jsonObject = JObject.Parse(varValue.ToString());
                    return jsonObject.SelectToken(path?.Remove(0, 2) ?? "/*") ?? null;
                }

                if (typeof(JToken).IsAssignableFrom(varType))
                {
                    var jsonToken = JToken.Parse(varValue.ToString());
                    return jsonToken.SelectToken(path?.Remove(0, 2) ?? "/*") ?? null;
                }

                if (typeof(XNode).IsAssignableFrom(varType))
                {
                    return ((XNode)varValue).XPathSelectElement(path ?? "/*");
                }

                if (typeof(XmlNode).IsAssignableFrom(varType))
                {
                    return ((XmlNode)varValue).SelectSingleNode(path ?? "/*");
                }

                try
                {
                    if (typeof(DataRow).IsAssignableFrom(varType))
                    {
                        if (keyPath == string.Empty)
                        {
                            return ((DataRow)varValue);
                        }
                        if (int.TryParse(keyPath, out int id))
                        {
                            return ((DataRow)varValue).ItemArray[id].ToString();
                        }
                        return ((DataRow)varValue)[keyPath].ToString();
                    }

                    if (!typeof(DataTable).IsAssignableFrom(varType))
                    {
                        return varValue;
                    }

                    if(!key.Contains("[") || !key.Contains("["))
                    {
                        return varValue;
                    }

                    if (!int.TryParse(key.Substring(key.IndexOf('[') + 1, key.IndexOf(']') - key.IndexOf('[') - 1), out index))
                    {
                        index = -1;
                    }

                    var row = ((DataTable)varValue).Rows[index];

                    var offset = key.IndexOf(']') + 1;

                    var tst = key.IndexOf('[', offset);

                    if (key.IndexOf('[', offset) < 0)
                    {
                        return row;
                    }

                    return int.TryParse(key.Substring(key.IndexOf('[', offset) + 1, key.IndexOf(']', offset) - key.IndexOf('[', offset) - 1), out index) ? row[index] : row[key.Substring(key.IndexOf('[', offset) + 1, key.IndexOf(']', offset) - key.IndexOf('[', offset) - 1)];
                }
                catch (IndexOutOfRangeException)
                {
                    Log.Logger().LogWarning($"Check the correctness of the key: \"{key}\"");
                    return null;
                }

            }catch (NullReferenceException)
            {
                Log.Logger().LogWarning($"Set NULL value in \"GetVariableValue\"/\"GetVariableValueText\"");
                return null;
            }
        }
        public string GetVariableValueText(string key)
        {
            var val = GetVariableValue(key);

            if (val == null)
            {
                return null;
            }

            string ret;
            switch (val)
            {
                case XElement element when element.HasElements == false:
                    {
                        ret = element.Value;
                        break;
                    }
                case XElement element when element.HasElements == true:
                    {
                        Log.Logger().LogWarning($"Key \"{key}\" is root for (XElement)");
                        using (var sw = new StringWriter())
                        {
                            element.Save(sw);
                            return sw.ToString();
                        }
                    }

                case XmlNode node when node.FirstChild.GetType() == typeof(XmlText):
                    {
                        ret = node.FirstChild.Value;
                        break;
                    }
                case XmlElement element when element.HasChildNodes == true:
                    {
                        Log.Logger().LogWarning($"Key \"{key}\" is root for (XmlElement)");
                        return null;
                    }

                case JToken token when token.HasValues == true:
                    {
                        ret = token.ToString();
                        break;
                    }

                default:
                    ret = Reflection.ConvertObject<string>(val);
                    break;
            }

            return ret;
        }
    }
}