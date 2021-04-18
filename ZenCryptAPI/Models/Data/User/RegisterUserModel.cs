using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenCryptAPI.Models.Data.User
{
    public class RegisterUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
