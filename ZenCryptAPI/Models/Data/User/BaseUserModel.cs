using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenCryptAPI.Models.Data.User
{
    public class BaseUserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
