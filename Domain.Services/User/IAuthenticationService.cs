using System.Threading.Tasks;
using Domain.DataTransferObjects.User;

namespace Domain.Services.User
{
    public interface IAuthenticationService
    {
        bool IsValidToken(string token);
        Task<Entities.SQL.User.User> GetUserFromToken(string token);
        string GetJsonWebToken(Entities.SQL.User.User user);
        Task<Entities.SQL.User.User> AuthenticateUser(LoginUserDTO user);

        Task<Entities.SQL.User.User> InsertUser(RegisterUserDTO user);
    }
}