using SocketLibrary.Contracts;

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
            var targetMethod = _routeConfig[messageTypeName];
            var response = targetMethod.Item2(targetMethod.Item1.Invoke(rawRequest));
            return response;
        }
    }
}