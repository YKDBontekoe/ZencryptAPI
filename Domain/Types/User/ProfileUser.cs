using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Frames.BoundObjects;

namespace Domain.Types.User
{
    public class ProfileUser
    {
        public Entities.SQL.User.User User { get; set; }
        public IEnumerable<Entities.SQL.User.User> Following { get; set; }   
        public IEnumerable<Entities.SQL.User.User> FollowedBy { get; set; }      
    }
}
