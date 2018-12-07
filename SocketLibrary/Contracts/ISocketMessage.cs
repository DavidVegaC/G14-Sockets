using System;

namespace SocketLibrary.Contracts
{
    public interface ISocketMessage
    {
        string CorrelationId { get; set; }
        Type MessageType { get; set; }
    }
}
