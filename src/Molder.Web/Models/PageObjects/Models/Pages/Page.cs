using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;
using Molder.Web.Models.Providers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Molder.Web.Models.PageObjects.Pages
{
    public class Page : BasePage
    {
        [ThreadStatic]
        private IDriverProvider _driverProvider;

        public override string Name { get; set; }
        public override string Url { get; set; }
        public override Node Root { get; set; }

        public Page()
        {
            Name = this.GetType().GetCustomAttribute<PageAttribute>()?.Name;
            Url = this.GetType().GetCustomAttribute<PageAttribute>()?.Url;

            // TODO
            Console.WriteLine();
        }

        public void SetProvider(IDriverProvider provider)
        {
            this._driverProvider = provider;
        }

        public override Block GetBlock(string name)
        {
            var block = Root.SearchElementBy(name, ObjectType.Block);

            (block.Object as Block).SetProvider(_driverProvider);
            (block.Object as Block).Root = block;
            return block.Object as Block;
        }

        public override IElement GetElement(string name)
        {
            var element = Root.SearchElementBy(name);
            (element.Object as Element).SetProvider(_driverProvider);
            (element.Object as Element).Root = element;
            return element.Object as IElement;
        }

        public override IEnumerable<IElement> GetPrimaryElements()
        {
            var elements = Root.Childrens.Where(c => (c.Object as Element).Optional == false);
            return elements.GetObjectFrom<IElement>();
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
            var frame = Root.SearchElementBy(name, ObjectType.Frame);

            (frame.Object as Frame).SetProvider(_driverProvider);
            (frame.Object as Frame).Root = frame;
            return frame.Object as Frame;
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
    }
}