namespace SocketLibrary.Contracts
{
    public interface ISocketMessage
    {
        string CorrelationId { get; set; }
        string MessageType { get; set; }        
    }
}
