using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.Mediator;
using Molder.Web.Models.Providers;
using Molder.Web.Models.Settings;
using OpenQA.Selenium;
using System;
using System.Linq;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;

namespace Molder.Web.Models.PageObjects.Frames
{
    public class Frame : Element
    {
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

        public override void SetProvider(IDriverProvider provider)
        {
            _provider = null;
            _frameMediator = new FrameMediator((provider.Settings as BrowserSetting).ElementTimeout);
            _driverProvider = GetFrame(provider);
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
            var block = Root.SearchElementBy(name, ObjectType.Block);

            (block.Object as Block).SetProvider(_driverProvider);
            (block.Object as Block).Root = block;
            return block.Object as Block;
        }

        public Frame GetFrame(string name)
        {
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame).SetProvider(_driverProvider);
            (frame.Object as Frame).Root = frame;
            return frame.Object as Frame;
        }

        public IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name);
            (element.Object as Element).SetProvider(_driverProvider);
            (element.Object as Element).Root = element;
            return element.Object as IElement;
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

            _driver = _frameMediator.Execute(() => provider.GetFrame(By.XPath(Locator))) as IDriverProvider;
            _driver.Settings = provider.Settings;
            return _driver;
        }
    }
}
