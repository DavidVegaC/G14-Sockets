using SocketLibrary.Contracts;
using System;
using System.Collections.Generic;

namespace SocketLibrary.Routing
{
    public class RoutingConfig
    {
        private readonly Dictionary<Type, Func<object, ISocketMessage>> _routes;

        public RoutingConfig()
        {
            _routes = new Dictionary<Type, Func<object, ISocketMessage>>();
        }

        public Func<object, ISocketMessage> this[Type index]
        {
            get { return _routes[index];  }
        }

        public void Register(Type messageType, Func<object, ISocketMessage> targetMethod)
        {
            _routes.Add(messageType, targetMethod);
        }
    }
}
