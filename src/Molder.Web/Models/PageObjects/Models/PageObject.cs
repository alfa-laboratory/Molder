using Molder.Exceptions;
using Molder.Models.Assembly;
using Molder.Models.Directory;
using Molder.Web.Extensions;
using Molder.Web.Infrastructures;
using Molder.Web.Models.PageObjects.Attributes;
using Molder.Web.Models.PageObjects.Blocks;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.PageObjects.Frames;
using Molder.Web.Models.PageObjects.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Molder.Web.Models
{
    public class PageObject
    {
        public IDirectory BaseDirectory { get; set; } = new BinDirectory();
        public IAssembly CustomAssembly { get; set; } = new Molder.Models.Assembly.Assembly();

        public IEnumerable<Node> Pages { get; private set; } = null;

        public PageObject()
        {
            Pages = Initialize(GetPages());
        }

        private IEnumerable<Node> GetPages()
        {
            var _projects = GetAssembly();
            var pages = new List<Node>();

            foreach (var project in _projects)
            {
                var classes = project.GetTypes().Where(t => t.IsClass).Where(t => t.GetCustomAttribute(typeof(PageAttribute), true) != null);

                foreach (var cl in classes)
                {
                    var pageAttribute = cl.GetCustomAttribute<PageAttribute>();
                    var page = (Page)Activator.CreateInstance(cl);
                    pages.Add(new Node
                    {
                        Name = pageAttribute.Name,
                        Object = page,
                        Type = ObjectType.Page,
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
                page.Childrens = GetChildrens(elements);
            }

            return _pages;
        }

        private IEnumerable<Node> GetChildrens(IEnumerable<FieldInfo> elements)
        {
            var childrens = new List<Node>();
            foreach(var element in elements)
            {
                var (name, type, obj) = GetElement(element);
                switch(type)
                {
                    case ObjectType.Element:
                        {
                            childrens.Add(new Node
                            {
                                Name = name,
                                Object = obj,
                                Type = type,
                                Childrens = null
                            });
                            break;
                        }
                    case ObjectType.Block:
                    case ObjectType.Frame:
                        {
                            var _elements = obj.GetType()
                                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .Where(f => f.GetCustomAttribute<ElementAttribute>() != null);
                            childrens.Add(new Node
                            {
                                Name = name,
                                Object = obj,
                                Type = type,
                                Childrens = GetChildrens(_elements)
                            });
                            break;
                        }
                }
            }
            return childrens;
        }

        private (string, ObjectType, object) GetElement(FieldInfo fieldInfo)
        {
            Attribute attribute;
            string name = string.Empty;
            object element = null;
            ObjectType objectType = ObjectType.Element;

            if (fieldInfo.CheckAttribute(typeof(BlockAttribute)))
            {
                attribute = fieldInfo.GetCustomAttribute<BlockAttribute>();
                name = ((BlockAttribute)attribute).Name;
                element = (Block)Activator.CreateInstance(fieldInfo.FieldType, ((BlockAttribute)attribute).Name, ((BlockAttribute)attribute).Locator, ((BlockAttribute)attribute).Optional);
                objectType = ObjectType.Block;
            }
            else
            {
                if (fieldInfo.CheckAttribute(typeof(FrameAttribute)))
                {
                    attribute = fieldInfo.GetCustomAttribute<FrameAttribute>();
                    name = ((FrameAttribute)attribute).Name;
                    element = (Frame)Activator.CreateInstance(fieldInfo.FieldType, ((FrameAttribute)attribute).Name, ((FrameAttribute)attribute).FrameName, ((FrameAttribute)attribute).Number, ((FrameAttribute)attribute).Locator, ((FrameAttribute)attribute).Optional);
                    objectType = ObjectType.Frame;
                }
                else
                {
                    attribute = fieldInfo.GetCustomAttribute<ElementAttribute>();
                    name = ((ElementAttribute)attribute).Name;
                    element = (Element)Activator.CreateInstance(fieldInfo.FieldType, ((ElementAttribute)attribute).Name, ((ElementAttribute)attribute).Locator, ((ElementAttribute)attribute).Optional);
                }
            }
            return (name, objectType, element);
        }

        private IEnumerable<System.Reflection.Assembly> GetAssembly()
        {
            var assemblies = new List<System.Reflection.Assembly>();

            BaseDirectory.Create();
            if (BaseDirectory.Exists())
            {
                var files = BaseDirectory.GetFiles("*.dll");
                foreach (var file in files)
                {
                    assemblies.Add(CustomAssembly.LoadFile(file.FullName));
                }
                return assemblies;
            }
            else
            {
                throw new DirectoryException($"BaseDirectory from path \"{BaseDirectory}\" is not exist");
            }
        }
    }
}