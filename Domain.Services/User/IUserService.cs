using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Services.User
{
    public interface IUserService
    {
        Task<Entities.SQL.User.User> GetUserByEmail(string userEmail);
        Task<T> GetUserById<T>(Guid userId, UserType userType); 
        Task<Entities.SQL.User.User> GetUsersByUserName(string userName);
    }
}
