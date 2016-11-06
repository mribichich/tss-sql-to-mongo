namespace ConsoleApplication2.Core
{
    using ConsoleApplication2.ReadModel.Handlers;

    public interface ICommandHandler<T> : IHandler<T>
        where T : ICommand
    {
    }
}