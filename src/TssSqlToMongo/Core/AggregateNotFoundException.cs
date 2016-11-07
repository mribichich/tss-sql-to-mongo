namespace TssSqlToMongo.Core
{
    using System;

    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(Guid id)
            : base($"Aggregate {id} was not found")
        {
        }
    }
}