using ExpertStore.Ordering.Domain;

namespace ExpertStore.Ordering.UseCases
{
    public class CreateOrder
    {
    }

    public class CreateOrderInput
    {
        public CreateOrderInput(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;    
        }

        public int ProductId { get; }
        public int Quantity { get; }
    }

    public class CreateOrderOutput
    {
        public CreateOrderOutput(Guid id, OrderStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; }
        public OrderStatus Status { get; }
    }
}
