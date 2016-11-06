namespace ConsoleApplication2.Core
{
    public interface IApplyEvent<in TEvent>
        where TEvent : IEvent
    {
        void Apply(TEvent e);
    }
}