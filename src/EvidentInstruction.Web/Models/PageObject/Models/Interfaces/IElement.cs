namespace EvidentInstruction.Web.Models.PageObject.Models.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        string Text { get; }
        object Value { get; }

        bool Loaded { get; }
        bool Enabled { get; }
        bool Displayed { get; }
        bool Selected { get; }
        bool Editabled { get; }

        void SetMediator(IMediator mediator);

        string GetAttribute(string name);

        //bool IsTextContains(string text);
        //bool IsTextEquals(string text);
        //
        //bool IsTextChange(string text);
        //bool IsValueChange(string text);
    }
}
