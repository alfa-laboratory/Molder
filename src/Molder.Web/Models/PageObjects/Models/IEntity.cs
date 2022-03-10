using Molder.Web.Infrastructures;
using OpenQA.Selenium;

namespace Molder.Web.Models.PageObjects
{
    public interface IEntity
    {
        Node Root { get; set; }
        string Name { get; set; }
        How How { get; set; }
        string Locator { get; set; }
        By By { get; }
        bool Optional { get; set; }
    }
}