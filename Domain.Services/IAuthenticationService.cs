using Domain.DataTransferObjects;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAuthenticationService
    {
        bool IsValidToken(string token);
        Task<User> GetUserFromToken(string token);
        string GetJsonWebToken(User user);
        Task<User> AuthenticateUser(BaseUserDTO user);

        Task<User> InsertUser(User user);
    }
}
