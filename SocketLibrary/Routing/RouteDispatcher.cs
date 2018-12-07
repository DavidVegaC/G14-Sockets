using SocketLibrary.Contracts;
using System;
using System.Diagnostics;

namespace SocketLibrary.Routing
{
    public class RouteDispatcher
    {
        private readonly RoutingConfig _routeConfig;

        public RouteDispatcher(RoutingConfig routeConfig)
        {
            _routeConfig = routeConfig;
        }

        public ISocketMessage Dispatch(string messageTypeName, string rawRequest)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            var messageType = Type.GetType(messageTypeName);

            var parameterInstance = Activator.CreateInstance(messageType);
            var method = messageType.GetMethod("Parse");
            var parameter = method.Invoke(parameterInstance, new object[] { rawRequest });

            var targetMethod = _routeConfig[messageType];
            stopwatch.Stop();

            Console.WriteLine($"RouteDispatcher {stopwatch.Elapsed}");

            stopwatch.Restart();
            var response = targetMethod(parameter);
            stopwatch.Stop();

            Console.WriteLine($"Method call {stopwatch.Elapsed}");
            return response;
        }
    }
}
