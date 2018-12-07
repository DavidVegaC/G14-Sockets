using SocketLibrary.Contracts;
using SocketLibrary.Serializer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketLibrary
{
    public delegate ISocketMessage DataReceivedCallback(string messageTypeName, string rawRequestString);

    public class SocketServer : Socket
    {
        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        private readonly DataReceivedCallback _dataReceivedCallback;
        private readonly Encoding _encoding;
        private readonly ISerializer _serializer;
        private readonly IPEndPoint _localEndpoint;
        private readonly int _connectionBacklog;

        public SocketServer(IPEndPoint localEndpoint,
                            DataReceivedCallback dataReceivedCallback,
                            int connectionBacklog = 10,
                            ISerializer serializer = null,
                            Encoding encoding = null)
            :base(localEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            _dataReceivedCallback = dataReceivedCallback;
            _encoding = encoding ?? Encoding.UTF8;
            _serializer = serializer ?? new JsonSerializer();
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
            handler.BeginReceive(state.BytesRead, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        { 
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;

            if (state.ByteBuffer == null)
            {
                state.MessageSize = BitConverter.ToInt32(new byte[]
                {
                    state.BytesRead[0],
                    state.BytesRead[1],
                    state.BytesRead[2],
                    state.BytesRead[3]
                }, 0);

                state.ByteBuffer = new byte[state.MessageSize];
            }
            
            var bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                var bytesToRead = StateObject.BufferSize > bytesRead ? bytesRead : StateObject.BufferSize;
                var srcOffset = state.ByteBuffer[0] == ControlCharacters.Null ? SocketMessageHeaders.MessageSizeEndIndex : 0;

                Buffer.BlockCopy(state.BytesRead, srcOffset, state.ByteBuffer, state.BufferOffset, bytesToRead - srcOffset);

                state.BufferOffset = state.BufferOffset + bytesToRead - srcOffset;

                if (state.BufferOffset == state.MessageSize)
                {
                    var rawRequestString = _serializer.GetString(state.ByteBuffer);
                    var messageTypeName = _serializer.GetValue(rawRequestString, "MessageType");
                    var response = _dataReceivedCallback(messageTypeName, rawRequestString);
                    Send(handler, _serializer.Serialize(response));
                }
                else
                {
                    handler.BeginReceive(state.BytesRead, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, byte[] byteData)
        {
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
