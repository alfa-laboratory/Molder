using Microsoft.Extensions.Logging;
using Molder.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace Molder.Web.Models.Proxy
{
    [ExcludeFromCodeCoverage]
    public class Proxy : IDisposable
    {
        private ProxyServer _proxyServer;
        private Dictionary<int, Authentication> _authentications;
        public Proxy()
        {
            _authentications = new Dictionary<int, Authentication>();
            _proxyServer = new ProxyServer();
            _proxyServer.BeforeRequest += OnRequest;
            _proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            _proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;

            _proxyServer.Start();
        }

        public int AddEndpoint(Authentication auth)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var conArr = ipGlobalProperties.GetActiveTcpListeners();

            for (var i = 50000; i < 60000; i++)
            {
                if (conArr.Any(x => x.Port == i)) continue;

                _proxyServer.AddEndPoint(new ExplicitProxyEndPoint(IPAddress.Any, i));

                _authentications.Add(i, auth);
                return i;
            }
            throw new Exception("Couldn't find any available tcp port!");
        }

        public Task OnRequest(object sender, SessionEventArgs e)
        {
            if (!_authentications.TryGetValue(e.ClientLocalEndPoint.Port, out var auth))
            {
                e.Ok("<html><h>Error with proxy</h></html>");
                return Task.CompletedTask;
            }

            Log.Logger().LogInformation(e.HttpClient.Request.Url);

            e.CustomUpStreamProxy = new ExternalProxy(auth.Proxy, auth.Port, auth.Username, auth.Password)
            {
                ProxyType = ExternalProxyType.Http
            };
            return Task.CompletedTask;
        }

        public Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                e.IsValid = true;

            return Task.CompletedTask;
        }

        public Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this._proxyServer.Dispose();
        }
    }
}