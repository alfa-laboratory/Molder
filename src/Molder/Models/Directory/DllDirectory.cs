using Microsoft.Extensions.Logging;
using Molder.Helpers;

namespace Molder.Models.Directory
{
    public class DllDirectory : DefaultDirectory
    {
        public override string Get()
        {
            Log.Logger().LogInformation("Work with DllDirectory");
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Substring(6);
        }
    }
}
