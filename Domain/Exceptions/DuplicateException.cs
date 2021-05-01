using System;

namespace Domain.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base("Item is already in database!")
        {
        }

        public DuplicateException(string entity) : base($"{entity} is already in database!")
        {
        }
    }
}