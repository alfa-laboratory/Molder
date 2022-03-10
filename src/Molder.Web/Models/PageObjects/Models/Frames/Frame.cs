using System.Threading;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.Mediator;
using Molder.Web.Models.Providers;
using OpenQA.Selenium;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.Settings;

namespace Molder.Web.Models.PageObjects.Frames
{
    public class Frame : Element
    {
        #region Frame Mediator

        private AsyncLocal<IMediator> _frameMediator = new AsyncLocal<IMediator>{ Value = null };

        protected new IMediator mediator
        {
            get => _frameMediator.Value;
            set => _frameMediator.Value = value;
        }

        #endregion
        
        protected string _frameName;
        protected int? _number;
        

        protected Frame(string name, string frameName, int? number, string locator, bool optional = false) : base(name, locator, optional)
        {
            _number = number;
            _frameName = frameName;
        }

        public new void SetProvider(IDriverProvider provider)
        {
            ElementProvider = null;
            mediator = new FrameMediator(BrowserSettings.Settings.Timeout);
            Driver = GetFrame(provider);
        }

        public IDriverProvider Parent()
        {
            return mediator.Execute(() => Driver.GetParentFrame()) as IDriverProvider;
        }

        public IDriverProvider Default()
        {
            return _frameMediator.Value.Execute(() => Driver.GetDefaultFrame()) as IDriverProvider;
        }

        public Block GetBlock(string name)
        {
            var block = Root.SearchElementBy(name, ObjectType.Block);

            (block.Object as Block)?.SetProvider(Driver);
            (block.Object as Block)?.Get();
            ((Block) block.Object).Root = block;
            return block.Object as Block;
        }

        public Frame GetFrame(string name)
        {
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame)?.SetProvider(Driver);
            ((Frame) frame.Object).Root = frame;
            return frame.Object as Frame;
        }

        public IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name);
            (element.Object as Element)?.SetProvider(Driver);
            (element.Object as Element)?.Get();
            ((Element) element.Object).Root = element;
            return (IElement) element.Object;
        }

        private IDriverProvider GetFrame(IDriverProvider provider)
        {
            IDriverProvider _driver = default;
            if (_frameName != null)
            {
                _driver = _frameMediator.Value.Execute(() => provider.GetFrame(_frameName)) as IDriverProvider;
                return _driver;
            }

            if (_number != null)
            {
                _driver = _frameMediator.Value.Execute(() => provider.GetFrame((int)_number)) as IDriverProvider;
                return _driver;
            }

            _driver = _frameMediator.Value.Execute(() => provider.GetFrame(By.XPath(Locator))) as IDriverProvider;
            return _driver;
        }
    }
}