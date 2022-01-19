using ExpertStore.SeedWork;

namespace ExpertStore.Ordering.Integration
{
    public class OrderCreatedEvent: IIntegrationEvent
    {
        public Guid OrderId { get; }
        public int ProductId { get; }
        public int Quantity { get; }
        public DateTime Date { get; }
    }
}
