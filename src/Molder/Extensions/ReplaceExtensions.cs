using Molder.Controllers;
using Molder.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Molder.Extensions
{
    public static class ReplaceExtensions
    {
        public static string ReplaceVariables(this VariableController variableController, string str, Func<object, string> foundReplace = null!, Func<object, string> notFoundReplace = null!)
        {
            return variableController.ReplaceVariables(str, StringPattern.SEARCH, foundReplace, notFoundReplace);
        }

        public static string ReplaceVariables(this VariableController variableController, string str, string pattern, Func<object, string> foundReplace = null!, Func<object, string> notFoundReplace = null!)
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

                    var (methodName, parameters) = ReplaceMethodsExtension.GetFunction(variable);

                    if (methodName is null)
                    {
                        if (variableController.GetVariable(variable) is null)
                        {
                            return notFoundReplace != null ? notFoundReplace(variable) : variable;
                        }

                        val = variableController.GetVariableValue(variable);
                        return (foundReplace != null ? foundReplace(val) : val.ToString())!;
                    }

                    var _params = Array.Empty<string>();
                    if(parameters is not null)
                    {
                        _params = new string[parameters.Length];
                        if (parameters.Any())
                        {
                            for(var i = 0; i < parameters.Length; i++)
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

                    var function = ReplaceMethodsExtension.Check(methodName);
                    if(function.GetParameters().Length != _params.Length)
                    {
                        return notFoundReplace != null ? notFoundReplace(variable) : variable;
                    }
                    var funcVal = ReplaceMethodsExtension.Invoke(methodName, _params);
                    return (foundReplace != null ? foundReplace(funcVal) : funcVal.ToString())!;
                },
                RegexOptions.None);
            return fmt;
        }
        
        public static Table ReplaceWith(this Table table, VariableController variableController)
        {
            var dt = new Table(table.Header.ToArray());
            table.Rows.ToList().ForEach(row =>
            {
                var tr = new List<string>();
                row.Values.ToList().ForEach(elem =>
                {
                    tr.Add(variableController.ReplaceVariables(elem));
                });
                dt.AddRow(tr.ToArray());
                tr.Clear();
            });
            return dt;
        }
    }
}