using System;

namespace Domain.Exceptions
{
    public class NoPermissionException : Exception
    {
        public NoPermissionException() : base("You don't have permission to perform this action!")
        {
        }

        public NoPermissionException(string action) : base($"You don't have permission to {action}")
        {
        }
    }
}