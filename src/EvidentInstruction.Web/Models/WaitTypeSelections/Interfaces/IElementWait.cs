namespace EvidentInstruction.Web.Models.WaitTypeSelections.Interfaces
{
    public interface IElementWait
    {
        bool ToBeVisible();
        bool ToBeInvisible();
        bool ToBeDisabled();
        bool ToBeEnabled();
        bool ToBeSelected();
        bool ToNotBeSelected();
    }
}
