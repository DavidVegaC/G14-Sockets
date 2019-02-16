using IPC.HTTP;
using IPC.HTTP.Contracts;
using System;
using System.Timers;

//Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
namespace SelfHostedWebsiteA.NET
{
    class Program
    {
        private static IIPCServer _ipcServer;
        private static IIPCClient _ipcClient;

        static void Main(string[] args)
        {
            var serverAddress = "http://localhost:10000/";
            var clientAddress = "http://localhost:11111/";

            _ipcServer = new WebApiServer(serverAddress);
            _ipcServer.StartListening();

            _ipcClient = new WebApiClient(clientAddress, "api/values");

            ClientSendRequest();

            Console.ReadLine();

            _ipcServer.Dispose();
            _ipcClient.Dispose();
        }

        public static void ClientSendRequest()
        {
            var timer = new Timer
            {
                Interval = 10000,                
                AutoReset = true,
                Enabled = true
            };

            timer.Elapsed += async (object sender, ElapsedEventArgs e) =>
            {
                Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Sending request");
                var response = await _ipcClient.Call<SampleMessage>(new SampleMessage { Message = "Client A" });
                //Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} {response.Message}");
            };            
        }
    }
}
