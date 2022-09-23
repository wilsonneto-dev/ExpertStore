using ExpertStore.Ordering.Domain;
using ExpertStore.SeedWork.Interfaces;

namespace ExpertStore.Ordering.UseCases;

public class ListOrders : IUseCase<List<ListOrdersOutputItem>>
{
    public ListOrders(IOrderRepository repository, ILogger<ListOrders> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ListOrdersOutputItem>> Handle()
    {
        var orders = await _repository.GetList();
        _logger.LogWarning("Orders listed");
        return orders.Select(ListOrdersOutputItem.FromOrder).ToList();
    }

    readonly IOrderRepository _repository;
    readonly ILogger<ListOrders> _logger;
}

public class ListOrdersOutputItem
{
    public ListOrdersOutputItem(Guid id, int productId, string status, int quantity, DateTime date)
    {
        Id = id;
        ProductId = productId;
        Status = status;
        Quantity = quantity;
        Date = date;
    }

    public static ListOrdersOutputItem FromOrder(Order order)
        => new(
            order.Id,
            order.ProductId,
            order.Status.ToString(),
            order.Quantity,
            order.Date
        );

    public Guid Id { get; }
    public int ProductId { get; }
    public string Status { get; }
    public int Quantity { get; }
    public DateTime Date { get; }
}