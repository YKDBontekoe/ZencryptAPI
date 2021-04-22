using System.Collections.Generic;

namespace Domain.Types.User
{
    public class ProfileUser
    {
        public Entities.SQL.User.User User { get; set; }
        public IEnumerable<Entities.SQL.User.User> Following { get; set; }
        public IEnumerable<Entities.SQL.User.User> FollowedBy { get; set; }
    }
}