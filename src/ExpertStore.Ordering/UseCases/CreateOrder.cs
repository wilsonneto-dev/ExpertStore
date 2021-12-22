using ExpertStore.Ordering.Domain;
using ExpertStore.SeedWork;

namespace ExpertStore.Ordering.UseCases
{
    public class CreateOrder: IUseCase<CreateOrderInput,CreateOrderOutput>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrder(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository; 
        }

        public async Task<CreateOrderOutput> Handle(CreateOrderInput input)
        {
            ValidateInput(input);
            var order = new Order(input.ProductId, input.Quantity);
            await _orderRepository.Save(order);
            return new CreateOrderOutput(order.Id, order.Status.ToString());
        }

        private void ValidateInput(CreateOrderInput input)
        {
            if (input.Quantity <= 0 || input.ProductId <= 0)
                throw new ArgumentException($"{nameof(input.Quantity)} e {nameof(input.ProductId)} devem ser positivos maiores que 0");
        }
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
        public CreateOrderOutput(Guid id, String status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; }
        public String Status { get; }
    }
}
