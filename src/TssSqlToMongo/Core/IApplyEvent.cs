namespace TssSqlToMongo.Core
{
    public interface IApplyEvent<in TEvent>
        where TEvent : IEvent
    {
        void Apply(TEvent e);
    }
}