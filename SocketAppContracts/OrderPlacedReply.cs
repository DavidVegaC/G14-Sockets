using SocketLibrary.Contracts;
using System;

namespace SocketAppContracts
{
    public class OrderPlacedReply : SocketMessage<OrderPlacedReply>
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
