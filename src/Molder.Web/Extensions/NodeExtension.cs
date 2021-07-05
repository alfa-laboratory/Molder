using Molder.Web.Exceptions;
using Molder.Web.Infrastructures;
using Molder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Molder.Web.Extensions
{
    public static class NodeExtension
    {
        public static Node SearchElementBy(this Node node, string name)
        {
            return (node.Childrens as List<Node>).SingleOrDefault(n => n.Type == ObjectType.Element && n.Name == name) ?? throw new SearchException($"A element \"{name}\" was not found in the {node.Type.ToString().ToLower()} \"{node.Name}\"");
        }

        public static Node SearchPageBy(this IEnumerable<Node> nodes, string name)
        {
            return (nodes as List<Node>).SingleOrDefault(n => n.Type == ObjectType.Page && n.Name == name) ?? throw new SearchException($"A page \"{name}\" was not found");
        }

        public static IEnumerable<T> GetObjectFrom<T>(this IEnumerable<Node> nodes)
        {
            var objects = new List<T>();
            (nodes as List<Node>).ForEach(node => objects.Add((T)node.Object));
            return objects;
        }

        public static Node SearchElementBy(this Node node, string name, ObjectType objectType)
        {
            var names = name.Split(SearchPattern.Separator, StringSplitOptions.None).ToList();
            return node.SearchBy(names, objectType);
        }

        private static Node SearchBy(this Node node, List<string> names, ObjectType objectType)
        {
            var _node = node;
            var _names = names;
            if (!_names.Any())
            {
                throw new SearchException($"A element for searching was empty");
            }
            else
            {
                foreach (var name in new List<string>(_names))
                {
                    _node =
                        (node.Childrens as List<Node>).SingleOrDefault(n => n.Type == objectType && n.Name == name) ??
                        throw new SearchException(
                            $"A element \"{name}\" was not found in the {_node.Type.ToString().ToLower()} \"{_node.Name}\"");
                    _names.Remove(name);
                    if (_names.Any())
                    {
                        return _node.SearchBy(_names, objectType);
                    }
                }
            }
            return _node;
        }
    }
}
