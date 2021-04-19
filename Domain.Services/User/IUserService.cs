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
        Task<TA> GetUserById<TA, TB, TC>(Guid userId, UserType userType)
           where TB : BaseEntity where TC : BaseEntity;
        Task<Entities.SQL.User.User> GetUsersByUserName(string userName);
        Task<Entities.SQL.User.User> FollowUser(string userToken, Guid userIdToFollow);
        Task<Entities.SQL.User.User> UnFollowUser(string userToken, Guid userIdToFollow);   
    }
}
