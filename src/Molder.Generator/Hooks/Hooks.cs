using Molder.Generator.Extensions;
using Molder.Helpers;
using Molder.Models.ReplaceMethod;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Molder.Controllers;
using Molder.Infrastructures;
using Molder.Models.Directory;
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

        [BeforeFeature(Order = -1)]
        public static void InitializePaths(VariableController variableController, FeatureContext featureContext)
        {
            // User
            var userDir = new UserDirectory().Get();
            var dir = $"{userDir}{Path.DirectorySeparatorChar}{featureContext.FeatureInfo.Title}";
            Directory.CreateDirectory(dir);
            variableController.SetPath(Infrastructures.Constants.USER_DIR, dir);
            
            // Bin
            
            var binDir = new BinDirectory().Get();
            variableController.SetPath(Infrastructures.Constants.BIN_DIR, binDir);
        }

        [AfterFeature]
        public static void RemoveDirectories(VariableController variableController)
        {
            var userDir = variableController.GetVariableValueText(Infrastructures.Constants.USER_DIR);
            if(Directory.Exists(userDir)) Directory.Delete(userDir, true);
        }
    }
}