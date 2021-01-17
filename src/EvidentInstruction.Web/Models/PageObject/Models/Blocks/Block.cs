using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using EvidentInstruction.Web.Helpers;
using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;

namespace EvidentInstruction.Web.Models.PageObject.Models.Blocks
{
    public class Block : Element
    {
        ConcurrentDictionary<string, IElement> _elements = new ConcurrentDictionary<string, IElement>();
        ConcurrentDictionary<string, Block> _blocks = new ConcurrentDictionary<string, Block>();
        ConcurrentDictionary<string, Frame> _frames = new ConcurrentDictionary<string, Frame>();

        public Block(string name, string locator, bool optional) : base(name, locator, optional) {  }

        public void Load(bool allElement = true)
        {
            if(allElement)
            {
                var elements = this.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                (_elements, _) = InitializeHelper.Elements(elements);

                var frames = this.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<FrameAttribute>() != null);
                _frames = InitializeHelper.Frames(frames);
            }
            var blocks = this.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.GetCustomAttribute<BlockAttribute>() != null);

            _blocks = InitializeHelper.Blocks(blocks);
        }

        public Block GetBlock(string name)
        {
            if (_blocks.Any())
            {
                var block = _blocks[name];
                block?.Load();
                return block;
            }
            return null;
        }
        
        public ConcurrentDictionary<string, Block> GetBlocks()
        {
            return _blocks;
        }

        public IElement GetElement(string name)
        {
            if (_elements.Any())
            {
                var element = _elements[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                ((Element)element).SetProvider(_driverProvider);
                return element;
            }
            throw new ArgumentOutOfRangeException($"List with all element for page {Name} is empty");
        }

        public IFrame GetFrame(string name)
        {
            if (_frames.Any())
            {
                var frame = _frames[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                frame.SetProvider(this._driverProvider);
                frame?.Load();
                _driverProvider = frame.Get();
                return frame as IFrame;
            }
            throw new ArgumentOutOfRangeException($"List with frames for frame {Name} is empty");
        }
    }
}
