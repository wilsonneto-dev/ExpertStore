namespace ExpertStore.Ordering.Domain
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetList();
        Task Save(Order order);
    }
}
