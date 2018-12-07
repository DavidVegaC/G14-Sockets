using SocketLibrary.Contracts;
using System;

namespace SocketAppContracts
{
    public class GetOrdersCommand : SocketMessage<GetOrdersCommand>
    {
        public string OrderId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
