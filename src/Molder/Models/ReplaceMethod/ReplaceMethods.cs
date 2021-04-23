using Molder.Extensions;
using System;
using System.Collections.Generic;

namespace Molder.Models.ReplaceMethod
{
    public class ReplaceMethods
    {
        private Lazy<IEnumerable<Type>> _types = new Lazy<IEnumerable<Type>>(() => new List<Type>());
        private static object syncRoot = new object();

        private ReplaceMethods()
        {
            (_types.Value as List<Type>).Add(typeof(ParseFunctions));
        }

        private static ReplaceMethods _instance;
        public static IEnumerable<Type> Get()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ReplaceMethods();
                    }
                }
            }
            return _instance._types.Value;
        }
    }
}