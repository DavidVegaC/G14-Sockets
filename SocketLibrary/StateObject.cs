using System.Net.Sockets;
using System.Text;

namespace SocketLibrary
{
    internal class StateObject
    {
        internal Socket workSocket = null;
        internal const int BufferSize = 232;
        internal int BufferOffset = 0;
        internal byte[] BytesRead = new byte[BufferSize];
        internal byte[] ByteBuffer;
        internal int MessageSize;
        internal StringBuilder sb = new StringBuilder();
    }
}
