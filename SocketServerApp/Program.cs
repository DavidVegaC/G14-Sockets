using SocketAppContracts;
using SocketLibrary;
using SocketLibrary.Routing;
using System;
using System.Net;

namespace SocketServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestDispatcher = new RequestDispatcher();

            var routingConfig = new RoutingConfig();
            routingConfig.Register(typeof(CreateCustomerCommand), requestDispatcher.CreateCustomer);
            routingConfig.Register(typeof(PlaceOrderCommand), requestDispatcher.PlaceOrder);

            var routeDispatcher = new RouteDispatcher(routingConfig);

            var server = new SocketServer(new IPEndPoint(IPAddress.Loopback, 11000),
                                          routeDispatcher.Dispatch);
            server.StartListening();

            Console.ReadKey();
        }
    }
}
