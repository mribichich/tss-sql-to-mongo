namespace TssSqlToMongo.Core
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event)
            where T : IEvent;

        ////Task PublishAsync<T>(T @event) where T : IEvent;
    }
}