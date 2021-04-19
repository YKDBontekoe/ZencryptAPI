using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.SQL.Forums;
using Domain.Enums;
using Domain.Enums.Neo;
using Domain.Services.Repositories;
using Domain.Services.User;
using Domain.Types.User;

namespace Services.User
{
    public class UserService : IUserService
    {
        private readonly ISQLRepository<Domain.Entities.SQL.User.User> _userSqlRepository;
        private readonly INeoRepository<Domain.Entities.SQL.User.User> _userNeoRepository;

        public UserService(ISQLRepository<Domain.Entities.SQL.User.User> userSqlRepository, INeoRepository<Domain.Entities.SQL.User.User> userNeoRepository)
        {
            _userSqlRepository = userSqlRepository;
            _userNeoRepository = userNeoRepository;
        }

        public Task<Domain.Entities.SQL.User.User> GetUserByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        /**
         * Finds user by user id
         * Returns user
         */
        public async Task<TA> GetUserById<TA, TB, TC>(Guid userId, UserType userType) where TB : BaseEntity where TC : BaseEntity
        {
            switch (userType)
            {
                case UserType.GENERAL:
                case UserType.MINIMAL:
                {
                    return (TA)(object) await _userSqlRepository.Get(userId);
                }

                case UserType.PROFILE:
                {
                    var foundUser = await _userSqlRepository.Get(userId);

                    var oneToManyFollowing = await _userNeoRepository.GetNodeWithRelatedNodes<TB, Domain.Entities.SQL.User.User>(userId, NEORelation.FOLLOWED, NeoRelationType.RIGHT);
                    var idsFollowing = oneToManyFollowing.ObjectList.Select(c => c.EntityId);

                    IEnumerable<Domain.Entities.SQL.User.User> following = new List<Domain.Entities.SQL.User.User>();
                    if (idsFollowing.Any())
                    {
                        following = await this._userSqlRepository.Filter(c => idsFollowing.Any(p => Guid.Parse(p) == c.Id));
                    }
                    

                    var oneToManyFollowedBy = await _userNeoRepository.GetNodeWithRelatedNodes<TB, Domain.Entities.SQL.User.User>(userId, NEORelation.FOLLOWED, NeoRelationType.LEFT);
                    var idsFollowedBy = oneToManyFollowedBy.ObjectList.Select(C => C.EntityId);

                    IEnumerable<Domain.Entities.SQL.User.User> followedBy = new List<Domain.Entities.SQL.User.User>();
                        if (idsFollowedBy.Any())
                    {
                        followedBy = await this._userSqlRepository.Filter(c =>
                            idsFollowedBy.Any(p => Guid.Parse(p) == c.Id));
                    }

                    var profileUser = new ProfileUser()
                    {
                        User = foundUser,
                        Following = following,
                        FollowedBy = followedBy
                    };

                    return (TA)(object)profileUser;
                }

                default:
                {
                    return (TA)(object) await _userSqlRepository.Get(userId);
                }
            }
        }

        public Task<Domain.Entities.SQL.User.User> GetUsersByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
