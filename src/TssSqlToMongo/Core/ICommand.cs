namespace TssSqlToMongo.Core
{
    using System;

    public interface ICommand : IMessage
    {
        Guid Id { get; }

        int? ExpectedVersion { get; }
    }
}