using Flurl.Http.Configuration;
using System.Net;
using System.Net.Http;

namespace Molder.Service.Models.ClientFactory
{
    public class CredentialHttpClientFactory : DefaultHttpClientFactory
    {
        public ICredentials CustomCredential { get; set; }

        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler { Credentials = CustomCredential };
        }
    }
}
