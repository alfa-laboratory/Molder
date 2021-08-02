using Molder.Generator.Extensions;
using Molder.Models.ReplaceMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace Molder.Generator.Hooks
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class Hooks : TechTalk.SpecFlow.Steps
    {
        [BeforeTestRun]
        public static void Initialize()
        {
            var obj = ReplaceMethods.Get() as List<Type>;
            if (!obj.Contains(typeof(GenerationFunctions)))
            {
                (ReplaceMethods.Get() as List<Type>).Add(typeof(GenerationFunctions));
            }
        }
    }
}