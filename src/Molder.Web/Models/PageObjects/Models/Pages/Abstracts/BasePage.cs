using System.Collections.Generic;
using System.Linq;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Frames;

namespace Molder.Web.Models.PageObjects.Pages
{
    public abstract class BasePage : IPage
    {
        public abstract string Url { get; set; }
        public abstract string Name { get; set; }
        public abstract Node Root { get; set; }

        public abstract Block GetBlock(string name);
        public abstract IElement GetElement(string name);
        public abstract IEnumerable<IElement> GetPrimaryElements();
        public abstract bool GoToPage();
        public abstract void PageTop();
        public abstract void PageDown();
        public bool IsLoadElements()
        {
            var errors = new List<string>();
            var elements = GetPrimaryElements();

            (elements as List<IElement>).ForEach(element =>
            {
                if (!element.Loaded)
                {
                    errors.Add(element.Name);
                }
            });

            if (elements.Any())
            {
                var aggregate = string.Join(", ", elements);
                Log.Logger().LogError($"элемент/ы \"{aggregate}\" не инициализированы на странице \"{Name}\"");
                return false;
            }
            else
            {
                return true;
            }
        }

        public abstract IPage GetDefaultFrame();
        public abstract Frame GetParentFrame();
        public abstract Frame GetFrame(string name);
    }
}
