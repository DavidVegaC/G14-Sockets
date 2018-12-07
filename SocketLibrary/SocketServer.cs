using SocketLibrary.Contracts;
using SocketLibrary.Serializer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketLibrary
{
    public class SocketServer : Socket
    {
        private const char _eof = '\0';

        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        private readonly Func<string, ISocketMessage> _dataReceivedCallback;
        private readonly Encoding _encoding;
        private readonly JsonSerializer _serializer;
        private readonly IPEndPoint _localEndpoint;
        private readonly int _connectionBacklog;

        public SocketServer(IPEndPoint localEndpoint,
                            Func<string, ISocketMessage> dataReceivedCallback,
                            int connectionBacklog = 10,
                            Encoding encoding = null)
            :base(localEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            _dataReceivedCallback = dataReceivedCallback;
            _encoding = encoding ?? Encoding.UTF8;
            _serializer = new JsonSerializer();
            _localEndpoint = localEndpoint;
            _connectionBacklog = connectionBacklog;
        }

        public void StartListening()
        { 
            try
            {
                Bind(_localEndpoint);
                Listen(_connectionBacklog);

                while (true)
                { 
                    _allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    BeginAccept(new AsyncCallback(AcceptCallback), this);

                    _allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _allDone.Set();

            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            var state = new StateObject
            {
                workSocket = handler
            };
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;
 
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;
  
            var bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(_encoding.GetString(state.buffer, 0, bytesRead));

                content = state.sb.ToString();
                if (content.EndsWith(_eof.ToString()))
                {
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                    content = content.TrimEnd('\0');
                    var response = _dataReceivedCallback(content);
                    Send(handler, _serializer.Serialize(response));
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, string data)
        {
            var byteData = _encoding.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
 
                var bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
