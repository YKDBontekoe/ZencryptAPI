using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenCryptAPI.Models.Data.User
{
    public class RegisterUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
