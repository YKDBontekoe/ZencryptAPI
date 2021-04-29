using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Services.User;

namespace Services
{
    public class TokenHandler
    {
        public static Task<Domain.Entities.SQL.User.User> TokenValidationAndReturnUser(IAuthenticationService authenticationService, string token)
                {
                    // Token validation
                    var isValidToken = authenticationService.IsValidToken(token);
        
                    // Check if token is valid
                    if (!isValidToken)
                        // Throw an exception if token is invalid
                        throw new InvalidTokenException();
        
                    // Get user from token
                    return authenticationService.GetUserFromToken(token);
                }
    }
}