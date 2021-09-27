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
            foreach(var (key, value) in variableController.Variables)
            {
                context.Add(key, value);
            }
            return context;
        }

        public static VariableController Reload(this VariableController variableController, SpecFlowContext context)
        {
            var controller = variableController;
            foreach(var (key, value) in context)
            {
                if(!controller.Variables.ContainsKey(key))
                {
                    if (value is Variable variable)
                    {
                        controller.Variables.TryAdd(key, variable);
                    }
                }
            }
            return controller;
        }
    }
}
