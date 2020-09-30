using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EvidentInstruction.Web.Models.Factory.Browser.Interfaces;
using EvidentInstruction.Web.Models.PageObject.Attributes;
using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;

namespace EvidentInstruction.Web.Models.PageObject.Models
{
    public class Page : IPage
    {
        private readonly ConcurrentDictionary<string, IElement> _allElements;
        private readonly IEnumerable<IElement> _primaryElemets;
        private readonly IEnumerable<IBlock> _blocks;

        private readonly IBrowser _browser;
        public Page(IBrowser browser)
        {
            _browser = browser;
            _allElements = new ConcurrentDictionary<string, IElement>();
            _primaryElemets = new List<IElement>();
        }











        public string Name => throw new System.NotImplementedException();

        public string Url => throw new System.NotImplementedException();

        public string BrowserUrl => throw new System.NotImplementedException();

        public string Title => throw new System.NotImplementedException();

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public IBlock GetBlock(string name)
        {
            throw new System.NotImplementedException();
        }

        public IElement GetElement(string name)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IElement> GetPrimaryElements()
        {
            throw new System.NotImplementedException();
        }

        public void GoToPage()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAppeared()
        {
            throw new System.NotImplementedException();
        }

        public bool IsDisappeared()
        {
            throw new System.NotImplementedException();
        }

        public void IsPageLoad()
        {
            throw new System.NotImplementedException();
        }

        public void Maximize()
        {
            throw new System.NotImplementedException();
        }

        public void PageDown()
        {
            throw new System.NotImplementedException();
        }

        public void PageTop()
        {
            throw new System.NotImplementedException();
        }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        public void SetWindowSize(int width, int height)
        {
            throw new System.NotImplementedException();
        }



        private void InitializeElements()
        {
            var fields = this.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null)
                        .ToArray();

            CollectAllPageElement(fields);
        }

        private void CollectAllPageElement(FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<ElementAttribute>();
                var locator = _context.ReplaceVariablesInXmlBody(attr.Locator);
                var element = (IElement)Activator.CreateInstance(field.FieldType, attr.Name, locator);

                var newFields = WebElement.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ElementAttribute>() != null)
                        .ToArray();

                if (!_allElements.ContainsKey(attr.Name))
                {
                    _allElements.Add(attr.Name, WebElement);
                }
                if (!attr.Optional)
                {
                    _primaryElemets.Add(WebElement);
                }

                if (newFields.Length > 0)
                {
                    WebElement = null;
                    CollectAllPageElement(newFields);
                }
                WebElement = null;
            }
        }


    }
}
