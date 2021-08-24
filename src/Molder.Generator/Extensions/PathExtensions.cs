using Molder.Controllers;
using Molder.Infrastructures;

namespace Molder.Generator.Extensions
{
    public static class PathExtensions
    {
        public static VariableController SetPath(this VariableController variableController, string key, string path)
        {
            var controller = variableController;
            controller.SetVariable(key, path.GetType(), path, TypeOfAccess.Global);
            return controller;
        }
    }
}