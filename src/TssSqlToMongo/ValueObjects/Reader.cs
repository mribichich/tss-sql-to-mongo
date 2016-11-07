namespace TssSqlToMongo.ValueObjects
{
    using System;

    public class Reader
    {
        public Reader(Guid id, int number)
        {
            this.Id = id;
            this.Number = number;
        }

        public Guid Id { get; protected set; }

        public int Number { get; protected set; }
    }
}