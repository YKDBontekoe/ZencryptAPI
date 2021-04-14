using System.Threading.Tasks;
using Domain.DataTransferObjects.User;

namespace Domain.Services.User
{
    public interface IAuthenticationService
    {
        bool IsValidToken(string token);
        Task<Entities.User.User> GetUserFromToken(string token);
        string GetJsonWebToken(Entities.User.User user);
        Task<Entities.User.User> AuthenticateUser(BaseUserDTO user);

        Task<Entities.User.User> InsertUser(Entities.User.User user);
    }
}
