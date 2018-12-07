using SocketLibrary.Contracts;

namespace SocketAppContracts
{
    public class CustomerCreatedReply : SocketMessage<CustomerCreatedReply>
    {
        public string CustomerId { get; set; }
    }
}
