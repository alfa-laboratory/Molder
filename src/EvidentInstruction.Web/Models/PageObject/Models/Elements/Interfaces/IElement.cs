namespace EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        string Text { get; }
        object Value { get; }

        bool Loaded { get; }
        bool Enabled { get;  }
        bool Displayed { get;  }
        bool Selected { get; }
        bool Editabled { get; }

        string GetAttribute(string name);
        void Move();
        void PressKey(string key);

        bool IsTextContains(string text);
        bool IsTextEquals(string text);
        bool IsTextMatch(string text);
    }
}
