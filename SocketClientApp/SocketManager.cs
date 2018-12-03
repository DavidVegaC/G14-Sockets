using SocketLibrary;
using System.Net;
using System.Net.Sockets;

namespace SocketClientApp
{
    public class SocketWrapper
    {
        private readonly IPEndPoint _remoteEndpoint;

        public SocketWrapper(IPEndPoint remoteEndpoint)
        {
            _remoteEndpoint = remoteEndpoint;
        }

        public object Call(string remoteMethodName)
        {
            object response;
            using (var socketClient = new SocketClient(_remoteEndpoint))
            {
                socketClient.StartClient();

                response = socketClient.Call(remoteMethodName);

                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();
            }
            return response;
        }
    }
}
