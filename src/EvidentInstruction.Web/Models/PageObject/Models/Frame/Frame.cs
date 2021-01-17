using EvidentInstruction.Web.Helpers;
using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Mediator;
using EvidentInstruction.Web.Models.PageObject.Models.Mediator.Interfaces;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using EvidentInstruction.Web.Models.Settings;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace EvidentInstruction.Web.Models.PageObject.Models
{
    public class Frame : Element
    {
        ConcurrentDictionary<string, IElement> _elements = new ConcurrentDictionary<string, IElement>();
        ConcurrentDictionary<string, Block> _blocks = new ConcurrentDictionary<string, Block>();
        ConcurrentDictionary<string, Frame> _frames = new ConcurrentDictionary<string, Frame>();

        [ThreadStatic]
        private IMediator _frameMediator = null;

        protected string _frameName;
        protected int? _number;

        public Frame() { }

        public Frame(string name, string frameName, int? number, string locator, bool optional = false) : base(name, locator, optional)
        {
            _number = number;
            _frameName = frameName;
        }

        public void Load(bool allElement = true)
        {
            if (allElement)
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

        public override void SetProvider(IDriverProvider provider)
        {
            _provider = null;
            _frameMediator = new FrameMediator((provider.Settings as BrowserSetting).ElementTimeout);
            _driverProvider = GetFrame(provider);
        }

        public ConcurrentDictionary<string, Block> GetBlocks()
        {
            return _blocks;
        }

        public ConcurrentDictionary<string, Frame> GetFrames()
        {
            return _frames;
        }

        public IDriverProvider Parent()
        {
            return _frameMediator.Execute(() => _driverProvider.GetParentFrame()) as IDriverProvider;
        }

        public IDriverProvider Default()
        {
            return _frameMediator.Execute(() => _driverProvider.GetDefaultFrame()) as IDriverProvider;
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

        public Frame GetFrame(string name)
        {
            if (_frames.Any())
            {
                var frame = _frames[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                frame.SetProvider(_driverProvider);
                frame?.Load();
                return frame;
            }
            throw new ArgumentOutOfRangeException($"List with frames for frame {Name} is empty");
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


        private IDriverProvider GetFrame(IDriverProvider provider)
        {
            IDriverProvider _driver = null;
            if (_frameName != null)
            {
                _driver = _frameMediator.Execute(() => provider.GetFrame(_frameName)) as IDriverProvider;
                _driver.Settings = provider.Settings;
                return _driver;
            }

            if (_number != null)
            {
                _driver = _frameMediator.Execute(() => provider.GetFrame((int)_number)) as IDriverProvider;
                _driver.Settings = provider.Settings;
                return _driver;
            }

            _driver = _frameMediator.Execute(() => provider.GetFrame(By.XPath(_locator))) as IDriverProvider;
            _driver.Settings = provider.Settings;
            return _driver;
        }
    }
}
