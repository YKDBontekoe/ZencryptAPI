using Domain.DataTransferObjects;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.DataTransferObjects.User;
using Domain.Entities.User;

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
