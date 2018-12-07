using SocketLibrary;
using SocketLibrary.Contracts;
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

        public T Call<T>(ISocketMessage socketMessage) where T : ISocketMessage
        {
            T response;
            using (var socketClient = new SocketClient(_remoteEndpoint))
            {
                socketClient.StartClient();

                response = socketClient.Call<T>(socketMessage);

                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();
            }
            return response;
        }
    }
}
