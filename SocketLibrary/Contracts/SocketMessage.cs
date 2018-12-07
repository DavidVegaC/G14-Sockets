using Newtonsoft.Json;
using System;

namespace SocketLibrary.Contracts
{
    public class SocketMessage<T> : ISocketMessage
    {
        private Type _messageType;

        public string CorrelationId { get; set; }
        public Type MessageType { get { return _messageType ?? GetType(); } set { _messageType = value; } }

        public T Parse(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
