using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Molder.Web.Models;

namespace Molder.Web.Tests.Models.PageObject
{
    [ExcludeFromCodeCoverage]
    public class TestPageObject : ITestPageObject
    {
        public IEnumerable<Node> Get()
        {
            return TreePages.Get();
        }
    }
}