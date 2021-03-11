using IPC.HTTP;
using IPC.HTTP.Contracts;
using System;
using System.Timers;

//Reference: https://docs.microsoft.com/en-us/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
namespace SelfHostedWebsiteB.NET
{
    class Program
    {
        private static IIPCServer _ipcServer;
        private static IIPCClient _ipcClient;

        static void Main(string[] args)
        {
            Console.Title = "Cliente A";

            var serverAddress = "http://localhost:11111/";
            var clientAddress = "http://localhost:10000/";

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
                Interval = 7000,
                AutoReset = true,
                Enabled = true
            };

            timer.Elapsed += async (object sender, ElapsedEventArgs e) =>
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{DateTime.UtcNow.ToString("ss.fff")} Enviado petición a Cliente B");
                var response = await _ipcClient.Call<SampleMessage>(new SampleMessage { Message = "Cliente A" });
            };
        }
    }
}
