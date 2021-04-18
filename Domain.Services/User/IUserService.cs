using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.User
{
    public interface IUserService
    {
        Task<Entities.SQL.User.User> GetUserByEmail(string userEmail);
        Task<Entities.SQL.User.User> GetUserById(Guid userId);
    }
}
