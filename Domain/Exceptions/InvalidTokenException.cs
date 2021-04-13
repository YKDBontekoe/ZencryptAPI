using System;

namespace Domain.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Token is invalid!")
        {
        }
    }
}