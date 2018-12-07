using SocketLibrary.Contracts;

namespace SocketAppContracts
{
    public class PlaceOrderCommand : SocketMessage<PlaceOrderCommand>
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string OrderItem { get; set; }
    }
}
