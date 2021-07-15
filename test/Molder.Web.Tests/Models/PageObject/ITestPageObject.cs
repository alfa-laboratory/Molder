using System.Collections.Generic;
using Molder.Web.Models;

namespace Molder.Web.Tests.Models.PageObject
{
    public interface ITestPageObject
    {
        IEnumerable<Node> Get();
    }
}