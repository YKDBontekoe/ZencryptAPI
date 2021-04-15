using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class CannotPerformActionException : Exception
    {
        public CannotPerformActionException() : base("We are unable to perform this action, sorry!")
        {
        }

        public CannotPerformActionException(string reason) : base($"We are unable to perform this action: {reason}")
        {
        }
    }
}
