namespace ConsoleApplication2.Core
{
    using System;

    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(Guid id)
            : base($"A different version than expected was found in aggregate {id}")
        {
        }
    }
}