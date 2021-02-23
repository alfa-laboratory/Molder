using Molder.Web.Infrastructures;
using System.Collections.Generic;

namespace Molder.Web.Models
{
    public class Node
    {
        public object Object { get; set; }
        public string Name { get; set; }
        public ObjectType Type { get; set; }

        public IEnumerable<Node> Childrens { get; set; }
    }
}