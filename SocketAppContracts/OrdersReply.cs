using SocketLibrary.Contracts;
using System;

namespace SocketAppContracts
{
    public class OrdersReply : SocketMessage<OrdersReply>
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
