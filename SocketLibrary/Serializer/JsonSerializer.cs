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
            if(bytes[bytes.Length - 1] == ControlCharacters.EndOfTransmission)
            {
                bytes[bytes.Length - 1] = ControlCharacters.Null;
            }
            return _encoding.GetString(bytes);
        }

        public byte[] Serialize(ISocketMessage socketMessage)
        {
            var jsonContent = JsonConvert.SerializeObject(socketMessage);
            var jsonBytes = _encoding.GetBytes(jsonContent);

            var bytes = new byte[jsonBytes.Length + 1];
            Buffer.BlockCopy(jsonBytes, 0, bytes, 0, jsonBytes.Length);
            bytes[bytes.Length - 1] = ControlCharacters.EndOfTransmission;
            return bytes;
        }

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes[bytes.Length - 1] == ControlCharacters.EndOfTransmission)
            {
                bytes[bytes.Length - 1] = ControlCharacters.Null;
            }
            var socketMessage = _encoding.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(socketMessage);
        }
    }
}
