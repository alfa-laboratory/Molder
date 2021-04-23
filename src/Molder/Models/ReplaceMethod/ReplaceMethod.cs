using Microsoft.Extensions.Logging;
using Molder.Extensions;
using Molder.Helpers;
using Molder.Infrastructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Molder.Models.ReplaceMethod
{
    public class ReplaceMethod : IReplace
    {
        public (string, string[]) GetFunction(string function)
        {
            var regex = new Regex(StringPattern.METHOD, RegexOptions.Compiled);
            var match = regex.Match(function);
            if(match.Success)
            {
                var method = match.Groups[StringPattern.MethodPlaceholder].Value;
                var parameters = match.Groups[StringPattern.ParametersPlaceholder].Value;
                return string.IsNullOrEmpty(parameters) ? (method, null) : (method, parameters.Split(','));
            }
            else
            {
                return (null, null);
            }
        }

        [ExcludeFromCodeCoverage]
        public object Invoke(string methodName, string[] parameters)
        {
            var method = Check(methodName);
            if (method is null) return null;

            return method.Invoke(null, parameters);
        }

        [ExcludeFromCodeCoverage]
        public MethodInfo Check(string methodName)
        {
            var methods = GetMethods();
            try
            {
                return methods.SingleOrDefault(m => m.Name == methodName);
            }catch(InvalidOperationException)
            {
                Log.Logger().LogWarning($"Found two or more methods with name \"{methodName}\" to execute in replace.");
                return null;
            }
        }

        [ExcludeFromCodeCoverage]
        protected IEnumerable<MethodInfo> GetMethods()
        {
            return typeof(ReplaceMethodExtensions).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).ToList();
        }
    }
}