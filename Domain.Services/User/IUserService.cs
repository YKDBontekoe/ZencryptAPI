using System;
using System.Threading.Tasks;
using Domain.DataTransferObjects.User;
using Domain.DataTransferObjects.User.Input;
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
        Task<FollowDTO> FollowUser(string userToken, CreateFollowInput createFollowInput);
        Task<UnfollowDTO> UnFollowUser(string userToken, RemoveFollowInput unfollowInput);
    }
}