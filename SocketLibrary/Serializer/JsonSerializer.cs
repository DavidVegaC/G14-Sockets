using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketLibrary.Contracts;
using System;
using System.Text;

namespace SocketLibrary.Serializer
{
    public class JsonSerializer : ISerializer
    {
        private readonly Encoding _encoding;

        /// <summary>
        /// Creates a new JsonSerializer
        /// </summary>
        /// <param name="encoding">Set encoding or use default Encoding.UTF8</param>
        public JsonSerializer(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }

        public string GetValue(string jsonString, string key)
        {
            var jObject = JObject.Parse(jsonString);
            return jObject[key].ToString();
        }

        public string GetString(byte[] bytes)
        {
            return _encoding.GetString(bytes);
        }

        public byte[] Serialize(ISocketMessage socketMessage)
        {
            var jsonContent = JsonConvert.SerializeObject(socketMessage);
            var jsonBytes = _encoding.GetBytes(jsonContent);

            var bytes = new byte[jsonBytes.Length + SocketMessageHeaders.MessageSizeEndIndex];
            var messageSizeBytes = BitConverter.GetBytes(jsonBytes.Length);

            //Set the message size using the first 4 bytes of the array.
            Buffer.BlockCopy(messageSizeBytes, 0, bytes, 0, messageSizeBytes.Length);

            //Fill the rest of the array with the socketMessage
            Buffer.BlockCopy(jsonBytes, 0, bytes, messageSizeBytes.Length, jsonBytes.Length);

            return bytes;
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var socketMessage = _encoding.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(socketMessage);
        }
    }
}
