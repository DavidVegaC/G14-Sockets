using System;
using System.Threading.Tasks;

namespace IPC.HTTP
{
    public interface IIPCClient : IDisposable
    {
        Task<T> Call<T>(object request, string uri = null);
        Task Send(object message, string uri = null);
    }
}
