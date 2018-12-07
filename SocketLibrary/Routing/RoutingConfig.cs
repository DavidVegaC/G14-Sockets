using SocketLibrary.Contracts;
using System;
using System.Collections.Generic;

namespace SocketLibrary.Routing
{
    public delegate ISocketMessage Parse(string content);
    public delegate ISocketMessage DispatchCommand(ISocketMessage command);

    public class RoutingConfig
    {
        private readonly Dictionary<string, Tuple<Parse, DispatchCommand>> _routes;

        public RoutingConfig()
        {
            _routes = new Dictionary<string, Tuple<Parse, DispatchCommand>>();
        }

        public Tuple<Parse, DispatchCommand> this[string index]
        {
            get { return _routes[index];  }
        }

        public void Register(string messageType, Tuple<Parse, DispatchCommand> targetMethod)
        {
            _routes.Add(messageType, targetMethod);
        }
    }
}
