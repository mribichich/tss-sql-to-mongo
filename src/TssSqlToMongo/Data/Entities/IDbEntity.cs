namespace ConsoleApplication2.Data.Entities
{
    using System;

    public interface IDbEntity
    {
        Guid Id { get; set; }

        int Version { get; set; }

        DateTime CreatedTimestamp { get; set; }

        string CreatedByUserId { get; set; }

        DateTime? LastModifiedTimestamp { get; set; }

        string LastModifiedByUserId { get; set; }
    }
}