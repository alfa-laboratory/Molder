using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Molder.Helpers;
using Molder.Web.Models.PageObject.Models.Blocks;
using Molder.Web.Models.PageObject.Models.Elements.Interfaces;
using Molder.Web.Models.PageObject.Models.Page.Interfaces;
using Microsoft.Extensions.Logging;

namespace Molder.Web.Models.PageObject.Models.Page.Abstracts
{
    public abstract class BasePage : IPage
    {
        protected ConcurrentDictionary<string, IElement> _allElements = new ConcurrentDictionary<string, IElement>();
        protected IEnumerable<IElement> _primaryElemets = new List<IElement>();
        protected ConcurrentDictionary<string, Block> _blocks = new ConcurrentDictionary<string, Block>();
        protected ConcurrentDictionary<string, Frame> _frames = new ConcurrentDictionary<string, Frame>();

        public abstract string Url { get; set; }
        public abstract string Name { get; set; }

        public abstract Block GetBlock(string name);

        public abstract IElement GetElement(string name);

        public abstract IEnumerable<IElement> GetPrimaryElements();

        public abstract bool GoToPage();

        public abstract void PageTop();

        public abstract void PageDown();

        public bool IsLoadElements()
        {
            var elements = new List<string>();

            _primaryElemets.ToList().ForEach(element =>
            {
                if (!element.Loaded)
                {
                    elements.Add(element.Name);
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
