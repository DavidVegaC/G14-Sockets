using SocketLibrary.Contracts;

namespace SocketAppContracts
{
    public class CreateCustomerCommand : SocketMessage<CreateCustomerCommand>
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string DateOfBirth { get; set; }
    }
}
