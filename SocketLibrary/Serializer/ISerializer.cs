using SocketLibrary.Contracts;

namespace SocketLibrary.Serializer
{
    public interface ISerializer
    {
        T Deserialize<T>(byte[] socketMessage);
        string GetString(byte[] bytes);
        string GetValue(string jsonString, string key);
        byte[] Serialize(ISocketMessage socketMessage);
    }
}