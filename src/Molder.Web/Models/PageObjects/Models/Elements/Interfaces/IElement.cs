using System.Collections.Generic;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Providers;

namespace Molder.Web.Models.PageObjects.Elements
{
    public interface IElement : IEntity
    {
        # region WebEntity
        IDriverProvider Driver { get; set; }
        IElementProvider ElementProvider { get; set; }
        #endregion
        #region Getters & Setters

        string Text { get; }
        string Tag { get; }
        object Value { get; }
        bool Loaded { get; }
        bool NotLoaded { get; }
        bool Enabled { get;  }
        bool Disabled { get; }
        bool Displayed { get;  }
        bool NotDisplayed { get; }
        bool Selected { get; }
        bool NotSelected { get; }
        bool Editabled { get; }
        bool NotEditable { get; }
        #endregion
        
        void SetProvider(IDriverProvider provider);
        public void Get();
        IElement Find(Node element, How how = How.XPath);
        IEnumerable<IElement> FindAll(Node element, How how = How.XPath);

        void Clear();
        string GetAttribute(string name);
        void Move();
        void PressKeys(string keys);

        bool IsTextContains(string text);
        bool IsTextEquals(string text);
        bool IsTextMatch(string text);

        object Clone();
    }
}