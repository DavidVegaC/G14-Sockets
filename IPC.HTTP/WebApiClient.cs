using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPC.HTTP
{
    public class WebApiClient : IIPCClient
    {
        private readonly string _serverAddress;
        private readonly string _defaultController;
        private readonly object _connectionLock = new object();
        private HttpClient _httpClient;

        public WebApiClient(string serverAddress, string defaultController = null)
        {
            _serverAddress = serverAddress;
            _defaultController = defaultController;
        }

        private void Connect()
        {
            lock (_connectionLock)
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }
            }
        }

        public async Task<T> Call<T>(object request, string controller = null)
        {
            Connect();

            var response = await _httpClient.PostAsJsonAsync(_serverAddress + _defaultController ?? controller, request);
            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        public async Task Send(object message, string controller = null)
        {
            Connect();
            await _httpClient.PostAsJsonAsync(_serverAddress + _defaultController ?? controller, message);
        }

        public void Dispose() => _httpClient?.Dispose();
    }
}
