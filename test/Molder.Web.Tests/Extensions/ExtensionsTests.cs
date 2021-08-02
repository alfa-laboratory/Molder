using Molder.Web.Models;
using Molder.Web.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using FluentAssertions;
using System.Diagnostics;
using System;

namespace Molder.Web.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ExtensionsTests
    {
        public ExtensionsTests() { }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 
                new List<Node>() {
                    new Node{
                        Name = "Page1",
                        Type = Infrastructures.ObjectType.Page,
                        Childrens = new List<Node>()
                        {
                            new Node{
                                Name = "Block1",
                                Type= Infrastructures.ObjectType.Block,
                                Childrens = new List<Node>(){
                                    new Node {
                                        Name = "Element1",
                                        Type = Infrastructures.ObjectType.Element
                                    }
                                }
                            }
                        }
                    }
                },
                "└───Page(Page1)" + Environment.NewLine +
                "|   └───Block(Block1)" + Environment.NewLine + 
                "|   |   |   Element(Element1)" + Environment.NewLine
            },

             new object[] {
                new List<Node>() {
                    new Node{
                    Name = "Page1",
                    Type = Infrastructures.ObjectType.Page,
                    Childrens = new List<Node>()
                    {
                        new Node{
                            Name = "Element1",
                            Type= Infrastructures.ObjectType.Element,
                        },
                        new Node{
                            Name = "Block1",
                            Type= Infrastructures.ObjectType.Block,
                            Childrens = new List<Node>(){
                                new Node {
                                    Name = "Element2",
                                    Type = Infrastructures.ObjectType.Element
                                }
                            }
                        },
                        new Node{
                            Name = "Element3",
                            Type= Infrastructures.ObjectType.Element,
                        },
                    }
                    }
                },
                "└───Page(Page1)" + Environment.NewLine +
                "|   |   Element(Element1)" + Environment.NewLine +
                "|   └───Block(Block1)" + Environment.NewLine +
                "|   |   |   Element(Element2)"+ Environment.NewLine +
                "|   |   Element(Element3)" + Environment.NewLine 
            }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void PageObjectToString(List<Node> pageObject, string actual)
        {
            var expected = LogPageObjectExtensions.PageObjectToString(pageObject);
            //Debug.WriteLine(expected);
            expected.Should().Be(actual);

        }
    }
}
