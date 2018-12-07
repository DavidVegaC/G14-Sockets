using Newtonsoft.Json;
using SocketLibrary.Contracts;

namespace SocketLibrary.Serializer
{
    public class JsonSerializer
    {
        public JsonSerializer()
        {

        }

        public string Serialize(ISocketMessage socketMessage)
        {
            return JsonConvert.SerializeObject(socketMessage);
        }

        public T Deserialize<T>(string socketMessage)
        {
            return JsonConvert.DeserializeObject<T>(socketMessage);
        }
    }
}
