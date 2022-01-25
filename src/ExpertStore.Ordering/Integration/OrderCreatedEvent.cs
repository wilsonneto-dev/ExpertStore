using ExpertStore.Ordering.Domain;
using ExpertStore.SeedWork;

namespace ExpertStore.Ordering.Integration
{
    public class OrderCreatedEvent: IIntegrationEvent
    {
        public OrderCreatedEvent(Order order)
        {
            Event = new OrderCreatedEventPayload
            {
                Date = order.Date,
                OrderId = order.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
            };
        }

        public object Event { get; set; }
    }

    public class OrderCreatedEventPayload
    {
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}


