using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Task<Domain.Entities.SQL.User.User> GetUserById(Guid userId)
        {
            return this._userSqlRepository.Get(userId);
        }
    }
}
