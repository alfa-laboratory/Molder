using Molder.Controllers;
using Molder.Helpers;
using Molder.Infrastructures;
using Molder.Models.ReplaceMethod;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Molder.Extensions
{
    public static class ReplaceExtensions
    {
        public static string ReplaceVariables(this VariableController variableController, string str, Func<object, string> foundReplace = null, Func<object, string> notFoundReplace = null)
        {
            return variableController.ReplaceVariables(str, StringPattern.SEARCH, foundReplace, notFoundReplace);
        }

        public static string ReplaceVariables(this VariableController variableController, string str, string pattern, Func<object, string> foundReplace = null, Func<object, string> notFoundReplace = null)
        {
            object val;
            var fmt = Regex.Replace(
                str ?? string.Empty,
                pattern,
                m =>
                {
                    if (m.Groups[1].Value.Length <= 0 || m.Groups[1].Value[0] == '(')
                    {
                        return $"{m.Groups[1].Value}";
                    }

                    var variable = m.Groups[1].Value;

                    var replaceMethod = new ReplaceMethod();
                    (string methodName, string[] parameters) = replaceMethod.GetFunction(variable);

                    if (methodName is null)
                    {
                        if (variableController.GetVariable(variable) is null)
                        {
                            return notFoundReplace != null ? notFoundReplace(variable) : variable;
                        }

                        val = variableController.GetVariableValueText(variable);
                        return foundReplace != null ? foundReplace(val) : val.ToString();
                    }
                    else
                    {
                        string[] _params = new string[0];
                        if(!(parameters is null))
                        {
                            _params = new string[parameters.Count()];
                            if (parameters.Any())
                            {
                                for(int i = 0; i < parameters.Count(); i++)
                                {
                                    if (variableController.GetVariable(parameters[i]) is null)
                                    {
                                        _params[i] = notFoundReplace != null ? notFoundReplace(parameters[i]) : parameters[i];
                                    }
                                    else
                                    {
                                        _params[i] = variableController.GetVariableValueText(parameters[i]);
                                    }
                                }
                            }
                        }

                        var function = replaceMethod.Check(methodName);
                        if(function.GetParameters().Count() != _params.Count())
                        {
                            return notFoundReplace != null ? notFoundReplace(variable) : variable;
                        }
                        var funcVal = replaceMethod.Invoke(methodName, _params);
                        return foundReplace != null ? foundReplace(funcVal) : funcVal.ToString();
                    }
                },
                RegexOptions.None);
            return fmt;
        }
    }
}
