using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Services.Repositories;
using Domain.Services.User;

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
        public async Task<T> GetUserById<T>(Guid userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.GENERAL:
                case UserType.MINIMAL:
                {
                    return (T)(object) await _userSqlRepository.Get(userId);
                }

                case UserType.PROFILE:
                {
                   // await _neoRepository.GetUserById<ProfileUser>(id, userType);
                }
                    break;
                default:
                {
                    return (T)(object) await _userSqlRepository.Get(userId);
                    }
            }

            return (T)(object)null;
        }

        public Task<Domain.Entities.SQL.User.User> GetUsersByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
