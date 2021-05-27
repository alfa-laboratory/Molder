using Molder.Controllers;
using Molder.Models;
using TechTalk.SpecFlow;

namespace Molder.Configuration.Extension
{
    public static class SpecFlowContextExtension
    {
        public static SpecFlowContext Copy(this SpecFlowContext specflowContext, VariableController variableController)
        {
            var context = specflowContext;
            foreach(var variable in variableController.Variables)
            {
                context.Add(variable.Key, variable.Value);
            }
            return context;
        }

        public static VariableController Reload(this VariableController variableController, SpecFlowContext context)
        {
            var controller = variableController;
            foreach(var variable in context)
            {
                if(!controller.Variables.ContainsKey(variable.Key))
                {
                    controller.Variables.TryAdd(variable.Key, variable.Value as Variable);
                }
            }
            return controller;
        }
    }
}
