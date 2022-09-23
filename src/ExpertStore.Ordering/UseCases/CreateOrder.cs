using ExpertStore.Ordering.Domain;
using ExpertStore.SeedWork.IntegrationEvents;
using ExpertStore.SeedWork.Interfaces;

namespace ExpertStore.Ordering.UseCases;

public class CreateOrder : IUseCase<CreateOrderInput, CreateOrderOutput>
{
    public CreateOrder(IOrderRepository orderRepository, IEventBus eventBus, ILogger<CreateOrder> logger)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<CreateOrderOutput> Handle(CreateOrderInput input)
    {
        ValidateInput(input);
        var order = new Order(input.ProductId, input.Quantity);
        await _orderRepository.Save(order);
        _eventBus.Publish(new OrderCreatedEvent(
            order.Date,
            order.Id,
            order.ProductId,
            order.Quantity));
        _logger.LogInformation("Order created! (log)");
        _logger.LogTrace("Order created! (trace)");
        return new CreateOrderOutput(order.Id, order.Status.ToString());
    }

    void ValidateInput(CreateOrderInput input)
    {
        if (input.Quantity <= 0 || input.ProductId <= 0)
            throw new ArgumentException($"{nameof(input.Quantity)} e {nameof(input.ProductId)} devem ser positivos maiores que 0");
    }

    readonly IOrderRepository _orderRepository;
    readonly IEventBus _eventBus;
    readonly ILogger<CreateOrder> _logger;
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
    public CreateOrderOutput(Guid id, string status)
    {
        Id = id;
        Status = status;
    }

    public Guid Id { get; }
    public string Status { get; }
}