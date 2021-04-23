using System.Reflection;

namespace Molder.Models.ReplaceMethod
{
    public interface IReplace
    {
        (string, string[]) GetFunction(string function);
        object Invoke(string methodName, string[] parameters);
        MethodInfo Check(string methodName);
    }
}