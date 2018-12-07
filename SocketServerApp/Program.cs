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
            var createCutomerCommand = new CreateCustomerCommand();
            routingConfig.Register(createCutomerCommand.MessageType, new Tuple<Parse, DispatchCommand>(createCutomerCommand.Parse, requestDispatcher.CreateCustomer));

            var placeOrderCommand = new PlaceOrderCommand();
            routingConfig.Register(placeOrderCommand.MessageType,
                new Tuple<Parse, DispatchCommand>(placeOrderCommand.Parse, requestDispatcher.PlaceOrder));
           
            var routeDispatcher = new RouteDispatcher(routingConfig);

            var server = new SocketServer(new IPEndPoint(IPAddress.Loopback, 11000),
                                          routeDispatcher.Dispatch);
            server.StartListening();

            Console.ReadKey();
        }
    }
}
