namespace ConsoleApplication2.Core
{
    using System;

    public abstract class BaseCommand : ICommand
    {
        protected BaseCommand(
            Guid id,
            int? expectedVersion,
            string userId)
        {
            this.Id = id;
            this.ExpectedVersion = expectedVersion;
            this.UserId = userId;
        }

        public Guid Id { get; protected set; }

        public int? ExpectedVersion { get; protected set; }

        public string UserId { get; protected set; }
    }
}