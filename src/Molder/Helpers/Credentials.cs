using System;
using System.Net;
using Molder.Infrastructures;

namespace Molder.Helpers
{
    public static class Credentials
    {
        public static CredentialCache CreateCredential(string host, AuthType authType, string domain, string username, string password)
        {
            var credentialCache = new CredentialCache();
            var networkCredential = new NetworkCredential(username, password, domain);
            credentialCache.Add(new Uri(host), authType.ToString(), networkCredential);
            return credentialCache;
        }
    }
}