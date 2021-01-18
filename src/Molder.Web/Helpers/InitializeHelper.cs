using Molder.Web.Models.PageObject.Attributes;
using Molder.Web.Models.PageObject.Models;
using Molder.Web.Models.PageObject.Models.Blocks;
using Molder.Web.Models.PageObject.Models.Elements;
using Molder.Web.Models.PageObject.Models.Elements.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Molder.Web.Helpers
{
    public static class InitializeHelper
    {
        public static ConcurrentDictionary<string, Frame> Frames(IEnumerable<FieldInfo> fields)
        {
            var dictionary = new ConcurrentDictionary<string, Frame>();

            foreach (var elementField in fields)
            {
                var attribute = elementField.GetCustomAttribute<FrameAttribute>();
                var element = (Frame)Activator.CreateInstance(elementField.FieldType, attribute.Name, attribute.FrameName, attribute.Number, attribute.Locator ,attribute.Optional );

                dictionary.TryAdd(attribute.Name, element);
            }

            return dictionary;
        }

        public static ConcurrentDictionary<string, Block> Blocks(IEnumerable<FieldInfo> fields)
        {
            var dictionary = new ConcurrentDictionary<string, Block>();

            foreach (var elementField in fields)
            {
                var attribute = elementField.GetCustomAttribute<BlockAttribute>();
                var element = (Block)Activator.CreateInstance(elementField.FieldType, attribute.Name, attribute.Locator, attribute.Optional );

                dictionary.TryAdd(attribute.Name, element);
            }

            return dictionary;
        }

        public static (ConcurrentDictionary<string, IElement> elements, IEnumerable<IElement> primary) Elements(IEnumerable<FieldInfo> fields)
        {
            var dictionary = new ConcurrentDictionary<string, IElement>();
            var primaryElements = new List<IElement>();

            foreach (var elementField in fields)
            {
                var attribute = elementField.GetCustomAttribute<ElementAttribute>();
                if (attribute is FrameAttribute) break;
                var element = (Element)Activator.CreateInstance(elementField.FieldType, attribute.Name, attribute.Locator, attribute.Optional );

                if (!attribute.Optional)
                {
                    primaryElements.Add(element);
                }

                dictionary.TryAdd(attribute.Name, element);
            }

            return (dictionary, primaryElements);
        }
    }
}
