using System;

namespace IPC.HTTP
{
    public interface IIPCServer : IDisposable
    {
        void StartListening();
    }
}
