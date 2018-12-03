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

            var socketManager = new SocketWrapper(new IPEndPoint(IPAddress.Loopback, 11000));
            
            var response = socketManager.Call("GetDateTime");

            stopWatch.Stop();

            Console.WriteLine($"Response received : {response} Elapsed: {stopWatch.Elapsed}");
            Console.ReadKey();
        }
    }
}
