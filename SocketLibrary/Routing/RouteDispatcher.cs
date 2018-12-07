using Newtonsoft.Json.Linq;
using SocketLibrary.Contracts;
using System;

namespace SocketLibrary.Routing
{
    public class RouteDispatcher
    {
        private readonly RoutingConfig _routeConfig;

        public RouteDispatcher(RoutingConfig routeConfig)
        {
            _routeConfig = routeConfig;
        }

        public ISocketMessage Dispatch(string rawRequest)
        {
            var messageTypeString = JObject.Parse(rawRequest)["MessageType"].ToString();
            var messageType = Type.GetType(messageTypeString);

            var parameterInstance = Activator.CreateInstance(messageType);
            var method = messageType.GetMethod("Parse");
            var parameter = method.Invoke(parameterInstance, new object[] { rawRequest });


            var targetMethod = _routeConfig[messageType];
            var response = targetMethod(parameter);
            return response;
        }
    }
}
