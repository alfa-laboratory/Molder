using Molder.Controllers;
using Molder.Web.Helpers;
using Molder.Web.Infrastructures;

namespace Molder.Web.Extensions
{
    public static class SettingExtension
    {
        public static VariableController AddSettings(this VariableController variableController)
        {
            var controller = variableController;

            var fields = SettingHelper.GetAllFields(typeof(DefaultSetting));

            foreach(var field in fields)
            {
                controller.SetVariable(field.Name, field.GetType(), field.GetValue(null), Molder.Infrastructures.TypeOfAccess.Default);
            }

            return controller;
        }
    }
}