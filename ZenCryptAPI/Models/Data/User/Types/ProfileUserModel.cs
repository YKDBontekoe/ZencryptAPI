using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenCryptAPI.Models.Data.User.Types
{
    public class ProfileUserModel
    {
        public FullUserModel User { get; set; }
        public ICollection<BaseUserModel> Following { get; set; }    
        public ICollection<BaseUserModel> FollowedBy { get; set; }

    }
}
