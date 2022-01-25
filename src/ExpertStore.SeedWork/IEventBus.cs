namespace ExpertStore.SeedWork;

public interface IEventBus
{
    void Publish(IIntegrationEvent @event);
}
