using Molder.Controllers;
using Molder.Web.Infrastructures;
using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;
using Molder.Web.Models.PageObjects.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Molder.Helpers;

namespace Molder.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class PageObject
    {
        private VariableController _variableController;

        public IEnumerable<Node> Pages { get; } = default;

        public PageObject(VariableController variableController)
        {
            _variableController = variableController;
            Pages = Initialize(GetPages());
        }

        private IEnumerable<Node> GetPages()
        {
            var projects = GetAssembly();
            if (projects is null) return null;
            
            var pages = new List<Node>();

            foreach (var project in projects)
            {
                var classes = project.GetTypes().Where(t => t.IsClass).Where(t => t.GetCustomAttribute(typeof(PageAttribute), true) != null);

                foreach (var cl in classes)
                {
                    var pageAttribute = cl.GetCustomAttribute<PageAttribute>();
                    var page = (Page)Activator.CreateInstance(cl);
                    page.SetVariables(_variableController);

                    pages.Add(new Node
                    {
                        Name = pageAttribute?.Name,
                        Object = page,
                        Type = ObjectType.Page,
                        Root = null,
                        Childrens = new List<Node>()
                    });
                }
            }

            return pages;
        }

        private IEnumerable<Node> Initialize(IEnumerable<Node> pages)
        {
            var _pages = pages;
            foreach(var page in _pages)
            {
                var elements = page.Object.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);
                page.Childrens = GetChildrens(elements, page);
            }

            return _pages;
        }
        
        private IEnumerable<Node> GetChildrens(IEnumerable<FieldInfo> elements, Node root, (object obj, ObjectType type)? rootObject = null)
        {
            var childrens = new List<Node>();
            foreach(var element in elements)
            {
                var (name, type, obj) = InitBy(element);
                switch(type)
                {
                    case ObjectType.Element:
                    {
                        if (rootObject is {type: ObjectType.Block})
                        {
                            if ((rootObject.Value.obj as Block)?.How is How.XPath && (obj as Element)?.How is How.XPath )
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + (obj as Element).Locator;
                                ((Element) obj).Locator = locator;
                            }
                        }
                        childrens.Add(new Node
                        {
                            Name = name,
                            Object = obj,
                            Type = type,
                            Root = root,
                            Childrens = null
                        });
                        break;
                    }
                    case ObjectType.Block:
                    {
                        var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);
                        
                        if (rootObject is {type: ObjectType.Block})
                        {
                            if ((rootObject.Value.obj as Block)?.How is How.XPath && (obj as Block)?.How is How.XPath )
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + (obj as Block).Locator;
                                ((Block) obj).Locator = locator;
                            }
                        }
                        
                        var node = new Node
                        {
                            Name = name,
                            Object = obj,
                            Type = type,
                            Root = root
                        };
                        node.Childrens = GetChildrens(subElements, node, (obj, ObjectType.Block));
                        childrens.Add(node);
                        
                        break;
                    }
                    case ObjectType.Frame:
                    {
                        var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                        if (rootObject is {type: ObjectType.Block})
                        {
                            if ((rootObject.Value.obj as Block)?.How is How.XPath && (obj as Frame)?.How is How.XPath )
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + (obj as Frame).Locator;
                                ((Frame) obj).Locator = locator;
                            }
                        }
                        
                        var node = new Node
                        {
                            Name = name,
                            Object = obj,
                            Type = type,
                            Root = root
                        };
                        node.Childrens = GetChildrens(subElements, node);
                        childrens.Add(node);
                        break;
                    }
                    case ObjectType.Collection:
                    {
                        var subElements = obj.GetType()
                            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);

                        if (rootObject is {type: ObjectType.Block})
                        {
                            if ((rootObject.Value.obj as Block)?.How is How.XPath && (obj as Block)?.How is How.XPath )
                            {
                                var locator = (rootObject.Value.obj as Block)?.Locator + (obj as Block).Locator;
                                ((Block) obj).Locator = locator;
                            }
                        }

                        var node = new Node
                        {
                            Name = name,
                            Object = obj,
                            Type = type,
                            Root = root
                        };
                        
                        node.Childrens = subElements.Any() ? GetChildrens(subElements, node) : null;
                        
                        childrens.Add(node);
                        break;
                    }
                    case { }:
                    {
                        break;
                    }
                }
            }
            return childrens;
        }

        private (string, ObjectType, object) InitBy(FieldInfo fieldInfo)
        {
            string name = default;
            object element = default;
            var objectType = ObjectType.Element;

            switch (fieldInfo.GetCustomAttribute<ElementAttribute>())
            {
                case CollectionAttribute collectionAttribute:
                {
                    name = collectionAttribute.Name;
                    element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        collectionAttribute.Name, collectionAttribute.Locator, collectionAttribute.Optional);
                    
                    (element as IElement).How = collectionAttribute.How;
                    objectType = ObjectType.Collection;
                    break;
                }
                
                case BlockAttribute blockAttribute:
                {
                    name = blockAttribute.Name;
                    element = Activator.CreateInstance(
                        fieldInfo.FieldType,
                        blockAttribute.Name, blockAttribute.Locator, blockAttribute.Optional);
                    (element as Block).How = blockAttribute.How;
                    objectType = ObjectType.Block;
                    break;
                }
                case FrameAttribute frameAttribute:
                {
                    name = frameAttribute.Name;
                    element = Activator.CreateInstance(fieldInfo.FieldType, 
                        frameAttribute.Name, frameAttribute.FrameName, frameAttribute.Number, frameAttribute.Locator, frameAttribute.Optional);
                    (element as Frame).How = frameAttribute.How;
                    objectType = ObjectType.Frame;
                    break;
                }
                case { } elementAttribute:
                {
                    name = elementAttribute.Name;
                    element = Activator.CreateInstance(fieldInfo.FieldType, 
                        elementAttribute.Name, elementAttribute.Locator, elementAttribute.Optional);
                    (element as Element).How = elementAttribute.How;
                    break;
                }
            }
            return (name, objectType, element);
        }

        private IEnumerable<Assembly> GetAssembly()
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies().ToList();
            }
            catch (Exception ex)
            {
                Log.Logger().LogError($@"Loading all assembly is failed, because {ex.Message}");
                return null;
            }
        }
    }
}