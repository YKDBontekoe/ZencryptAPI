using System.Threading.Tasks;
using Domain.DataTransferObjects.User.Input;

namespace Domain.Services.User
{
    public interface IAuthenticationService
    {
        bool IsValidToken(string token);
        Task<Entities.SQL.User.User> GetUserFromToken(string token);
        string GetJsonWebToken(Entities.SQL.User.User user);
        Task<Entities.SQL.User.User> AuthenticateUser(LoginUserInput user);

        Task<Entities.SQL.User.User> InsertUser(RegisterUserInput user);
    }
}