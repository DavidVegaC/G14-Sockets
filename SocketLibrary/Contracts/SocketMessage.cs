using Newtonsoft.Json;

namespace SocketLibrary.Contracts
{
    public class SocketMessage<T> : ISocketMessage
    {
        private string _messageType;

        public string CorrelationId { get; set; }
        public string MessageType { get { return _messageType ?? GetType().FullName; } set { _messageType = value; } }

        public T Parse(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
