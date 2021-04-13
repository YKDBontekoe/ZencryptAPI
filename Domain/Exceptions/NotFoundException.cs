using System;

namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Item has not been found!")
        {
        }

        public NotFoundException(string message) : base($"{message} has not been found!")
        {
        }
    }
}