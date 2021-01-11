using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Helpers;
using EvidentInstruction.Web.Infrastructures;

namespace EvidentInstruction.Web.Extensions
{
    public static class SettingExtension
    {
        public static VariableController AddSettings(this VariableController variableController)
        {
            var controller = variableController;

            var fields = SettingHelper.GetAllFields(typeof(DefaultSetting));

            foreach(var field in fields)
            {
                controller.SetVariable(field.Name, field.GetType(), field.GetValue(null), EvidentInstruction.Infrastructures.TypeOfAccess.Default);
            }

            return controller;
        }
    }
}