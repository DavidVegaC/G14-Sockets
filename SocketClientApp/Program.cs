using SocketAppContracts;
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
            

            var socketWrapper = new SocketWrapper(new IPEndPoint(IPAddress.Loopback, 11000));

            while (true)
            {
                stopWatch.Restart();
                var response = socketWrapper.Call<OrderPlacedReply>(new PlaceOrderCommand
                {
                    CustomerName = "Peter Brigs",
                    OrderItem = "Asus 34\" monitor",
                    CustomerId = "ABC123",
                    CorrelationId = Guid.NewGuid().ToString(),
                });

                stopWatch.Stop();
                Console.WriteLine($"Elapsed: {stopWatch.Elapsed}");
            }


            Console.ReadKey();
        }
    }
}
