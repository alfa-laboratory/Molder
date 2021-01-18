using Molder.Exceptions;
using Molder.Models.Directory;
using Molder.Models.Directory.Interfaces;
using Molder.Web.Models.PageObject.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Molder.Web.Helpers
{
    public static class BrowserHelper
    {
        [ThreadStatic]
        private static IEnumerable<Assembly> _projects = null;

        public static IDirectory BaseDirectory { get; set; } = new BaseDirectory();
        public static Molder.Models.Assembly.Interfaces.IAssembly CustomAssembly { get; set; } = new Molder.Models.Assembly.Assembly();

        public static IDictionary<string, Type> CollectPages()
        {
            _projects = GetAssembly();
            Dictionary<string, Type> allClasses = new Dictionary<string, Type>();

            foreach (var project in _projects)
            {
                var classes = project.GetTypes().Where(t => t.IsClass).Where(t => t.GetCustomAttribute(typeof(PageAttribute), true) != null);

                foreach (var cl in classes)
                {
                    allClasses.Add(cl.GetCustomAttribute<PageAttribute>().Name, cl);
                }
            }

            return allClasses;
        }

        private static IEnumerable<Assembly> GetAssembly()
        {
            var assemblies = new List<Assembly>();

            BaseDirectory.Create();
            if (BaseDirectory.Exists())
            {
                var files = BaseDirectory.GetFiles("*.dll");
                foreach (var file in files)
                {
                    assemblies.Add(CustomAssembly.LoadFile(file.FullName));
                }
                return assemblies;
            }else
            {
                throw new DirectoryException($"BaseDirectory from path \"{BaseDirectory}\" is not exist");
            }
        }
    }
}
