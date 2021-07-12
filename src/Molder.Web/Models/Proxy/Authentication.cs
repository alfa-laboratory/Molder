using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Proxy
{
    [ExcludeFromCodeCoverage]
    public class Authentication
    {
        public string Proxy { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}