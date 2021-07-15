using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Molder.Web.Exceptions;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models;
using Xunit;

namespace Molder.Web.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class NodeExtensionTests
    {
        [Fact]
        public void SearchElementBy_ByValidName_ReturnNode()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Element
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            var searchNode = node.SearchElementBy("Test1");
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedNode);
        }
        
        [Fact]
        public void SearchElementBy_ByInvalidType_ReturnException()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1"
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            Action act = () => node.SearchElementBy("Test1");
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("A element \"Test1\" was not found in the page \"test\"");
        }
        
        [Fact]
        public void SearchElementBy_ByManyEqualNode_ReturnException()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Element
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    expectedNode
                }
            };
            
            // Arrange
            
            Action act = () => node.SearchElementBy("Test1");
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("A page \"test\" contains more than one node named \"Test1\"");
        }
        
        [Fact]
        public void SearchPageBy_ByValidName_ReturnNode()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test",
                Type = ObjectType.Page
            };

            var listNode = new List<Node>
            {
                expectedNode,
                new Node()
                {
                    Name = "Test1",
                    Type = ObjectType.Page
                }
            };
            
            // Arrange

            var searchNode = listNode.SearchPageBy("Test");
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedNode);
        }
        
        [Fact]
        public void SearchPageBy_ByInvalidType_ReturnException()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test",
                Type = ObjectType.Element
            };

            var listNode = new List<Node>
            {
                expectedNode,
                new Node()
                {
                    Name = "Test1",
                    Type = ObjectType.Page
                }
            };
            
            // Arrange

            Action act = () => listNode.SearchPageBy("Test");
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("A page \"Test\" was not found");
        }
        
        [Fact]
        public void SearchPageBy_ByManyEqualNode_ReturnException()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test"
            };

            var listNode = new List<Node>
            {
                expectedNode,
                expectedNode
            };
            
            // Arrange
            
            Action act = () => listNode.SearchPageBy("Test");
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("The PageObject list contains multiple pages named \"Test\"");
        }
        
        [Fact]
        public void GetObjectFrom_ByValidName_ReturnListObjects()
        {
            // Act
            var expectedListObjects = new List<int>() {1, 2}; 
            
            var listNode = new List<Node>
            {
                new Node()
                {
                    Name = "Test",
                    Type = ObjectType.Page,
                    Object = 1
                },
                new Node()
                {
                    Name = "Test1",
                    Type = ObjectType.Page,
                    Object = 2
                }
            };
            
            // Arrange

            var searchNode = listNode.GetObjectFrom<int>();
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedListObjects);
        }
        
        [Fact]
        public void GetObjectFrom_ByNullObject_ReturnNullList()
        {
            // Act
            var expectedListObjects = new List<int?>() {null, null}; 
            
            var listNode = new List<Node>
            {
                new Node()
                {
                    Name = "Test",
                    Type = ObjectType.Page
                },
                new Node()
                {
                    Name = "Test1",
                    Type = ObjectType.Page
                }
            };
            
            // Arrange

            var searchNode = listNode.GetObjectFrom<int?>();
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedListObjects);
        }
        
        [Fact]
        public void GetObjectFrom_ByNullAndBadType_ReturnNull()
        {
            // Act
          
            var listNode = new List<Node>
            {
                new Node()
                {
                    Name = "Test",
                    Type = ObjectType.Page
                },
                new Node()
                {
                    Name = "Test1",
                    Type = ObjectType.Page
                }
            };
            
            // Arrange

            var nullList = listNode.GetObjectFrom<int>();
            
            // Assert
            nullList.Should().BeNull();
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        [Fact]
        public void SearchElementByNameAndType_ByValidNameAndType_ReturnNode()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Element
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            var searchNode = node.SearchElementBy("Test1", ObjectType.Element);
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedNode);
        }
        
        [Fact]
        public void SearchElementByNameAndType_ByValidNameAndTypeByLevel_ReturnNode()
        {
            // Act
            var expectedNode = new Node()
                {
                    Name = "Test11",
                    Type = ObjectType.Element
                };
                
            var nodeNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Element,
                Childrens = new List<Node>()
                {
                    expectedNode
                }
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    nodeNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            var searchNode = node.SearchElementBy("Test1..Test11", ObjectType.Element);
            
            // Assert
            searchNode.Should().BeEquivalentTo(expectedNode);
        }
        
        [Fact]
        public void SearchElementByNameAndType_ByInvalidSearchType_ReturnException()
        {
            // Act
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Element
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            Action act = () => node.SearchElementBy("Test1", ObjectType.Frame);
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("A frame \"Test1\" was not found in the page \"test\"");
        }
        
        [Fact]
        public void SearchElementByNameAndType_ByInvalidElementType_ReturnException()
        {
            // Act
            var expectedNode = new Node()
            {
                Name = "Test1",
                Type = ObjectType.Frame
            };
            
            var node = new Node()
            {
                Name = "test",
                Object = null,
                Type = ObjectType.Page,
                Childrens = new List<Node>()
                {
                    expectedNode,
                    new Node()
                    {
                        Name = "Test2"
                    }
                }
            };
            
            // Arrange

            Action act = () => node.SearchElementBy("Test1", ObjectType.Element);
            
            // Assert
            act
                .Should().Throw<SearchException>()
                .WithMessage("A element \"Test1\" was not found in the page \"test\"");
        }
    }
}