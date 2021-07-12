using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Molder.Web.Extensions;
using Molder.Web.Models.PageObjects.Attributes;
using Xunit;

namespace Molder.Web.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ClassWithAttr
    {
        [Block(Name = "Test")] public object Obj;
    }

    [ExcludeFromCodeCoverage]
    public class ClassWithoutAttr
    {
        public object Obj;
    }

    [ExcludeFromCodeCoverage]
    public class FieldExtensionTests
    {
        [Fact]
        public void CheckAttribute_CustomClassWithAttr_ReturnTrue()
        {
            var fld = typeof(ClassWithAttr)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();
            fld.CheckAttribute(typeof(BlockAttribute)).Should().BeTrue();
        }
        
        [Fact]
        public void CheckAttribute_CustomClassWithoutAttr_ReturnFalse()
        {
            var fld = typeof(ClassWithoutAttr)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();
            fld.CheckAttribute(typeof(BlockAttribute)).Should().BeFalse();
        }
    }
}