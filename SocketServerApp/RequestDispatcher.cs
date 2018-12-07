using SocketAppContracts;
using SocketLibrary.Contracts;
using System;

namespace SocketServerApp
{
    public class RequestDispatcher
    {
        public ISocketMessage CreateCustomer(ISocketMessage socketMessage)
        {
            var createCustomerCommand = (CreateCustomerCommand)socketMessage;

            return new CustomerCreatedReply
            {
                CorrelationId = createCustomerCommand.CorrelationId,
                CustomerId = Guid.NewGuid().ToString()
            };
        }

        public ISocketMessage PlaceOrder(ISocketMessage socketMessage)
        {
            var placeOrderCommand = (PlaceOrderCommand)socketMessage;

            return new OrderPlacedReply
            {
                CorrelationId = placeOrderCommand.CorrelationId,
                CustomerId = placeOrderCommand.CustomerId,
                OrderId = "OIX-111-120618",
                PlacedAt = DateTime.Now
            };
        }

        public ISocketMessage GetOrders(ISocketMessage socketMessage)
        {
            var getOrdersCommand = (GetOrdersCommand)socketMessage;

            return new OrdersReply
            {
                CorrelationId = getOrdersCommand.CorrelationId,
                CustomerName = "John Doe",
                OrderId = getOrdersCommand.OrderId,
                PlacedAt = DateTime.Now.AddMonths(-2)
            };
        }
    }
}
