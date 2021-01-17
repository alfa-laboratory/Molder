using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EvidentInstruction.Web.Helpers;
using EvidentInstruction.Web.Infrastructures;
using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;
using EvidentInstruction.Web.Models.PageObject.Models.Elements.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Models.Page.Abstracts;
using EvidentInstruction.Web.Models.PageObject.Models.Page.Interfaces;
using EvidentInstruction.Web.Models.Providers.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace EvidentInstruction.Web.Models.PageObject.Models.Page
{
    public class Page : BasePage
    {
        [ThreadStatic]
        private IDriverProvider _driverProvider;

        public override string Name { get; set; }
        public override string Url { get; set; }

        public Page()
        {
            Name = this.GetType().GetCustomAttribute<PageAttribute>()?.Name;
            Url = this.GetType().GetCustomAttribute<PageAttribute>()?.Url;
     
            var blocks = this.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<BlockAttribute>() != null);

            _blocks = InitializeHelper.Blocks(blocks);

            var frames = this.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<FrameAttribute>() != null);

            _frames = InitializeHelper.Frames(frames);

            var elements = this.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

            (_allElements, _primaryElemets) = InitializeHelper.Elements(elements);

            // TODO
            Console.WriteLine();
        }

        public void SetProvider(IDriverProvider provider)
        {
            this._driverProvider = provider;
        }

        public override Block GetBlock(string name)
        {
            var blocks = name.Split(new string[] { BlockStringPattern.BLOCKS }, StringSplitOptions.None).ToList();

            if (blocks.Count > 1)
            {
                return GetBlock(blocks, _blocks);
            }
            else
            {
                return GetBlock(name);
            }
        }

        public override IElement GetElement(string name)
        {
            if (_allElements.Any())
            {
                var element =  _allElements[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                ((Element)element).SetProvider(_driverProvider);
                return element;
            }
            throw new ArgumentOutOfRangeException($"List with all element for page {Name} is empty");
        }

        public override IEnumerable<IElement> GetPrimaryElements()
        {
            return _primaryElemets;
        }

        #region Работа с фреймами

        public override IPage GetDefaultFrame()
        {
            var frame = new Frame();
            frame.SetProvider(_driverProvider);
            _driverProvider = frame.Default();
            return this;
        }

        public override Frame GetParentFrame()
        {
            var frame = new Frame();
            frame.SetProvider(_driverProvider);
            _driverProvider = frame.Parent();
            return frame;
        }

        public override Frame GetFrame(string name)
        {
            var frames = name.Split(new string[] { BlockStringPattern.BLOCKS }, StringSplitOptions.None).ToList();

            if (frames.Count > 1)
            {
                return GetFrame(frames, _frames);
            }
            else
            {
                return GetFrame(name);
            }
        }

        #endregion

        public override bool GoToPage()
        {
            return _driverProvider.GoToUrl(Url);
        }

        public override void PageTop()
        {
            var action = new Actions(_driverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.Home).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public override void PageDown()
        {
            var action = new Actions(_driverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        #region Обработка sub блоков (временно до создания Non binary tree
        private Block GetBlock(string name, bool allElement = true)
        {
            if(_blocks.Any())
            {
                var block = _blocks[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                block.Load(allElement);
                block.SetProvider(_driverProvider);
                return block;
            }
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        private Block GetBlock(List<string> _names, ConcurrentDictionary<string, Block> _blocks)
        {
            var names = _names;
            var blocks = _blocks;
            
            foreach(var name in names)
            {
                if(blocks.Any())
                {
                    var block = blocks[name];
                    if(block != null)
                    {
                        names.Remove(name);
                        if(names.Any())
                        {
                            block.Load(false);
                            blocks = block.GetBlocks();
                            return GetBlock(names, blocks);
                        }
                        else
                        {
                            block.Load();
                            block.SetProvider(_driverProvider);
                            return block;
                        }
                    }
                    throw new ArgumentOutOfRangeException(nameof(name));
                }
            }
            throw new ArgumentOutOfRangeException("List with blocks is empty");
        }
        #endregion

        #region Обработка sub фреймов (временно до создания Non binary tree
        private Frame GetFrame(string name, bool allElement = true)
        {
            if (_frames.Any())
            {
                var frame = _frames[name] ?? throw new ArgumentOutOfRangeException(nameof(name));
                frame?.Load(allElement);
                frame.SetProvider(_driverProvider);
                return frame;
            }
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        private Frame GetFrame(List<string> _names, ConcurrentDictionary<string, Frame> _frames)
        {
            var names = _names;
            var frames = _frames;

            foreach (var name in names)
            {
                if (frames.Any())
                {
                    var frame = frames[name];
                    if (frame != null)
                    {
                        names.Remove(name);
                        if (names.Count > 1)
                        {
                            frame.Load(false);
                            frames = frame.GetFrames();
                            return GetFrame(names, frames);
                        }
                        else
                        {
                            frame.Load();
                            frame.SetProvider(_driverProvider);
                            return frame;
                        }
                    }
                    throw new ArgumentOutOfRangeException(nameof(name));
                }
            }
            throw new ArgumentOutOfRangeException("List with frames is empty");
        }
        #endregion
    }
}