using System.Collections.Concurrent;
using System.Collections.Generic;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces;

namespace EvidentInstruction.Web.Models.PageObject.Models.Page.Abstracts
{
    public abstract class BasePage : IPage
    {
        protected ConcurrentDictionary<string, IElement> _allElements = new ConcurrentDictionary<string, IElement>();
        protected IEnumerable<IElement> _primaryElemets = new List<IElement>();
        protected ConcurrentDictionary<string, Block> _blocks = new ConcurrentDictionary<string, Block>();
        protected ConcurrentDictionary<string, Frame> _frames = new ConcurrentDictionary<string, Frame>();

        public abstract Block GetBlock(string name);

        public abstract IElement GetElement(string name);

        public abstract IEnumerable<IElement> GetPrimaryElements();

        public abstract bool GoToPage();

        public abstract void PageTop();

        public abstract void PageDown();

        public bool IsLoad()
        {
            /// TODO
            return true;
        }
    }
}
