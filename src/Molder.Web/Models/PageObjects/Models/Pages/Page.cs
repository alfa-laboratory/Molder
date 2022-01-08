using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Molder.Controllers;
using Molder.Extensions;
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
        public override string Name => GetType().GetCustomAttribute<PageAttribute>()?.Name;
        public override string Url => _variableController.ReplaceVariables(GetType().GetCustomAttribute<PageAttribute>()?.Url);
        public override Node Root { get; set; }

        public void SetProvider(IDriverProvider provider)
        {
            DriverProvider = provider;
        }

        public void SetVariables(VariableController variableController) => _variableController = variableController;

        public override Block GetBlock(string name)
        {
            var root = Local ?? Root;
            var block = root.SearchElementBy(name, ObjectType.Block);
            (block.Object as Block)?.SetProvider(DriverProvider);
            (block.Object as Block)?.Get();
            ((Block) block.Object).Root = block;
            Local = block;
            return (Block) block.Object;
        }

        public override IElement GetElement(string name)
        {
            var root = Local ?? Root;
            var element = root.SearchElementBy(name);
            ((IElement) element.Object).Root = element;
            ((IElement) element.Object).SetProvider(DriverProvider);
            ((IElement) element.Object).Get();
            return (IElement) element.Object;
        }

        public override IEnumerable<IElement> GetCollection(string name)
        {
            var root = Local ?? Root;
            var collection = root.SearchCollectionBy(name);
            var elements = DriverProvider.GetElements(((IElement) collection.Object).Locator,
                ((IElement) collection.Object).How);
            var lst = new List<IElement>();

            foreach (var element in elements)
            {
                IElement obj = (IElement)((IElement) collection.Object).Clone();
                obj.Root = root;
                obj.SetProvider(DriverProvider);
                obj.ElementProvider = element;
                lst.Add(obj);
            }
            return lst;
        }

        public override IEnumerable<string> GetPrimaryElements()
        {
            var elements = Root.Childrens.Where(c => ((IElement) c.Object).Optional == false);
            return elements.Select(element => element.Name).ToList();
        }

        #region Работа с фреймами

        public override IPage GetDefaultFrame()
        {
            if (Local is not {Type: ObjectType.Frame}) return this;
            
            DriverProvider = (Local.Object as Frame)?.Default();
            Local = null;
            return this;
        }

        public override Frame GetParentFrame()
        {
            throw new System.NotImplementedException();
        }

        public override Frame GetFrame(string name)
        {
            var root = Local ?? Root;
            var frame = root.SearchElementBy(name, ObjectType.Frame);
            (frame.Object as Frame)?.SetProvider(DriverProvider);
            ((Frame) frame.Object).Root = frame;
            Local = frame;
            return frame.Object as Frame;
        }

        #endregion

        public override void GoToPage()
        {
            _driverProvider.GoToUrl(Url);
        }

        public override void PageTop()
        {
            var action = new Actions(DriverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.Home).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }

        public override void PageDown()
        {
            var action = new Actions(DriverProvider.GetDriver());
            action.SendKeys(Keys.Control).SendKeys(Keys.End).Build().Perform();
            action.KeyUp(Keys.Control).Perform();
        }
    }
}