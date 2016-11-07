namespace TssSqlToMongo.Core
{
    using TssSqlToMongo.ReadModel.Handlers;

    public interface ICommandHandler<T> : IHandler<T>
        where T : ICommand
    {
    }
}