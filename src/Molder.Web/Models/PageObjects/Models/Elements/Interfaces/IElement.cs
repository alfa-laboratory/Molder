namespace Molder.Web.Models.PageObjects.Elements
{
    public interface IElement
    {
        Node Root { get; set; }

        string Name { get; set; }
        bool Optional { get; }
        string Locator { get; }

        string Text { get; }
        string Tag { get; }
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