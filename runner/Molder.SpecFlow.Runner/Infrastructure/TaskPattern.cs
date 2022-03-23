using System;
using System.Collections.Generic;
using System.Linq;

namespace Molder.SpecFlow.Runner.Infrastructure
{
    public class TaskPattern
    {
        private IEnumerable<string> _tags = new[]
        {
            "Jira",
            "Zephyr"
        };

        private static readonly Lazy<TaskPattern> lazy =
            new(() => new TaskPattern());

        public static TaskPattern Get => lazy.Value;

        private TaskPattern() { }

        //\((.*)\)
        public string Pattern()
        {
            var str = string.Join("|", _tags.Select(i => i + @"\((.+)\)"));
            return $"^({str})";
        }

        //^(Jira\((Order\((.*)\)\)))
        public string Order()
        {
            var str = string.Join("|", _tags.Select(i => i + @"\((Order\((.*)\))\)"));
            return $"^({str})";
        }
    }
}