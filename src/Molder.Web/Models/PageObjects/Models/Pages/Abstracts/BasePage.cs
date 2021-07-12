using System.Collections.Generic;
using System.Linq;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Frames;
using Molder.Controllers;
using Molder.Web.Models.Providers;

namespace Molder.Web.Models.PageObjects.Pages
{
    public abstract class BasePage : IPage
    {
        protected VariableController _variableController;
        protected IDriverProvider _driverProvider;
        
        public IDriverProvider DriverProvider
        {
            get => _driverProvider;
            set => _driverProvider = value;
        }
        
        public abstract string Url { get; }
        public abstract string Name { get; }
        public abstract Node Root { get; set; }
        public virtual Node Local { get; set; } = null; 

        public abstract Block GetBlock(string name);
        public void BackToPage() => Local = null;

        public abstract IElement GetElement(string name);
        public abstract IEnumerable<IElement> GetPrimaryElements();
        public abstract void GoToPage();
        public abstract void PageTop();
        public abstract void PageDown();
        public bool IsLoadElements()
        {
            var errors = new List<string>();
            var elements = GetPrimaryElements();

            (elements as List<IElement>)?.ForEach(element =>
            {
                if (!element.Loaded)
                {
                    errors.Add(element.Name);
                }
            });

            if (elements.Any())
            {
                var aggregate = string.Join(", ", elements);
                Log.Logger().LogError($"element/s \"{aggregate}\" not initialize on page \"{Name}\"");
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