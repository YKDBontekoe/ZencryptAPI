using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Neo;
using Domain.Exceptions;
using Domain.Services.Repositories;
using Domain.Services.User;
using Domain.Types.User;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INeoRepository<Domain.Entities.SQL.User.User> _userNeoRepository;
        private readonly ISQLRepository<Domain.Entities.SQL.User.User> _userSqlRepository;

        public UserService(ISQLRepository<Domain.Entities.SQL.User.User> userSqlRepository,
            INeoRepository<Domain.Entities.SQL.User.User> userNeoRepository,
            IAuthenticationService authenticationService)
        {
            _userSqlRepository = userSqlRepository;
            _userNeoRepository = userNeoRepository;
            _authenticationService = authenticationService;
        }

        public Task<Domain.Entities.SQL.User.User> GetUserByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        /**
         * Finds user by user id
         * Returns user
         */
        public async Task<TA> GetUserById<TA, TB, TC>(Guid userId, UserType userType)
            where TB : BaseEntity where TC : BaseEntity
        {
            switch (userType)
            {
                case UserType.GENERAL:
                case UserType.MINIMAL:
                {
                    return (TA) (object) await _userSqlRepository.Get(userId);
                }

                case UserType.PROFILE:
                {
                    var foundUser = await _userSqlRepository.Get(userId);
                    IEnumerable<Domain.Entities.SQL.User.User> following = new List<Domain.Entities.SQL.User.User>();
                    IEnumerable<Domain.Entities.SQL.User.User> followedBy = new List<Domain.Entities.SQL.User.User>();
                    var oneToManyFollowing =
                        await _userNeoRepository.GetNodeWithRelatedNodes<TB, Domain.Entities.SQL.User.User>(userId,
                            NEORelation.FOLLOWED, NeoRelationType.RIGHT);

                    if (oneToManyFollowing != null)
                    {
                        var idsFollowing = oneToManyFollowing.ObjectList.Select(c => Guid.Parse(c.EntityId));
                        var users = await _userSqlRepository.GetAll();
                        following = users.Where(c => idsFollowing.Contains(c.Id));
                    }

                    var oneToManyFollowedBy =
                        await _userNeoRepository.GetNodeWithRelatedNodes<TB, Domain.Entities.SQL.User.User>(userId,
                            NEORelation.FOLLOWED, NeoRelationType.LEFT);
                    if (oneToManyFollowedBy != null)
                    {
                        var idsFollowedBy = oneToManyFollowedBy.ObjectList.Select(c => Guid.Parse(c.EntityId));
                        var users = await _userSqlRepository.GetAll();
                        followedBy = users.Where(c => idsFollowedBy.Contains(c.Id));
                    }

                    var profileUser = new ProfileUser
                    {
                        User = foundUser,
                        Following = following,
                        FollowedBy = followedBy
                    };

                    return (TA) (object) profileUser;
                }

                default:
                {
                    return (TA) (object) await _userSqlRepository.Get(userId);
                }
            }
        }

        public Task<Domain.Entities.SQL.User.User> GetUsersByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.SQL.User.User> FollowUser(string userToken, Guid userIdToFollow)
        {
            // Validates Token
            ValidateToken(userToken);

            // Find user
            var foundUser = await _userSqlRepository.Get(userIdToFollow);

            // Check if post is null
            if (foundUser == null)
                // Throw exception if there aren't any users
                throw new NotFoundException("user");

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(userToken);

            // Create relation in graph database
            await _userNeoRepository.CreateRelation(tokenUser, NEORelation.FOLLOWED, foundUser);

            // Return followed user
            return foundUser;
        }

        public async Task<Domain.Entities.SQL.User.User> UnFollowUser(string userToken, Guid userIdToFollow)
        {
            // Validates Token
            ValidateToken(userToken);

            // Find user
            var foundUser = await _userSqlRepository.Get(userIdToFollow);

            // Check if post is null
            if (foundUser == null)
                // Throw exception if there aren't any users
                throw new NotFoundException("user");

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(userToken);

            // Remove relation in graph database
            await _userNeoRepository.RemoveRelation(tokenUser, NEORelation.FOLLOWED, foundUser);

            // Return un- followed user
            return foundUser;
        }

        private void ValidateToken(string token)
        {
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
                // Throw an exception if token is invalid
                throw new InvalidTokenException();
        }
    }
}