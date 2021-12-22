using ExpertStore.Ordering.Domain;

namespace ExpertStore.Ordering.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly List<Order> _orders;

        public OrderRepository()
            => _orders = new List<Order>();

        public Task<List<Order>> GetList()
            => Task.FromResult(_orders.ToList());

        public Task Save(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }
    }
}
