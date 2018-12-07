using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using SocketLibrary.Contracts;
using SocketLibrary.Serializer;

namespace SocketLibrary
{
    public class SocketClient : Socket
    {
        private readonly IPEndPoint _remoteEndPoint;
        private readonly Encoding _encoding;
        private readonly JsonSerializer _serializer;

        private readonly ManualResetEvent connectDone = new ManualResetEvent(false);
        private readonly ManualResetEvent sendDone = new ManualResetEvent(false);
        private readonly ManualResetEvent receiveDone = new ManualResetEvent(false);
        
        public SocketClient(IPEndPoint remoteEndPoint, Encoding encoding = null)
            : base(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            _remoteEndPoint = remoteEndPoint;
            _encoding = encoding ?? Encoding.UTF8;
            _serializer = new JsonSerializer();
        }

        public void StartClient()
        {
            try
            {                 
                BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), this);
                connectDone.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;
 
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                                
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public T Call<T>(ISocketMessage socketMessage) where T : ISocketMessage
        {
            Send(socketMessage);
            return Receive<T>();            
        }

        private T Receive<T>() where T : ISocketMessage
        {
            try
            {
                var state = new StateObject
                {
                    workSocket = this
                };

                BeginReceive(state.BytesRead, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                receiveDone.WaitOne();

                return _serializer.Deserialize<T>(state.ByteBuffer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            { 
                var state = (StateObject)ar.AsyncState;
                var client = state.workSocket;
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

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    var bytesToRead = StateObject.BufferSize > bytesRead ? bytesRead : StateObject.BufferSize;
                    var srcOffset = state.ByteBuffer[0] == ControlCharacters.Null ? SocketMessageHeaders.MessageSizeEndIndex : 0;

                    Buffer.BlockCopy(state.BytesRead, srcOffset, state.ByteBuffer, state.BufferOffset, bytesToRead - srcOffset);

                    state.BufferOffset = state.BufferOffset + bytesToRead - srcOffset;

                    if (state.BufferOffset == state.MessageSize)
                    {
                        receiveDone.Set();
                    }
                    else
                    {
                        client.BeginReceive(state.BytesRead, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(ISocketMessage socketMessage)
        {
            var byteData = _serializer.Serialize(socketMessage);

            BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this);

            sendDone.WaitOne();
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
