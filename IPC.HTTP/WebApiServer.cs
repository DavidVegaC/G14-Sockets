using Microsoft.Owin.Hosting;
using System;

namespace IPC.HTTP
{
    public class WebApiServer : IIPCServer
    {
        private readonly string _serverAddress;
        private IDisposable _webHost;

        public WebApiServer(string serverAddress) => _serverAddress = serverAddress;

        public void Dispose() => _webHost.Dispose();

        public void StartListening() => _webHost = WebApp.Start<WebApiServerStartup>(url: _serverAddress);
    }
}