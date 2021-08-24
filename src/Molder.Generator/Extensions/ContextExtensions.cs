using TechTalk.SpecFlow;

namespace Molder.Generator.Extensions
{
    public static class ContextExtensions
    {
        public static SpecFlowContext AddOrUpdate(this SpecFlowContext context, string key, object value)
        {
            var cntxt = context;
            if (cntxt.ContainsKey(key))
            {
                cntxt.Remove(key);
            }
            cntxt.Add(key, value);
            return cntxt;
        }
    }
}