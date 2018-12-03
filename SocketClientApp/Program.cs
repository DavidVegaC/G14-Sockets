using System;
using System.Diagnostics;
using System.Net;

namespace SocketClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var socketManager = new SocketWrapper(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            
            var response = socketManager.Call("GetDateTime<EOF>");

            stopWatch.Stop();

            Console.WriteLine($"Response received : {response} Elapsed: {stopWatch.Elapsed}");
            Console.ReadKey();
        }
    }
}
