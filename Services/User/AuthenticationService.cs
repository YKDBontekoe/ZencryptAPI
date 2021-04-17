using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.DataTransferObjects.User;
using Domain.Exceptions;
using Domain.Services.Repositories;
using Domain.Services.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services.User
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly ISQLRepository<Domain.Entities.SQL.User.User> _userSqlRepository;
        private readonly INeoRepository<Domain.Entities.SQL.User.User> _userNeoRepository;

        public AuthenticationService(IConfiguration config, ISQLRepository<Domain.Entities.SQL.User.User> userSqlRepository, INeoRepository<Domain.Entities.SQL.User.User> userNeoRepository)
        {
            _config = config;
            _userSqlRepository = userSqlRepository;
            _userNeoRepository = userNeoRepository;
        }


        /**
         * Validates token given by @param and returns bool;
         * true is valid
         * false is invalid
         */
        public bool IsValidToken(string token)
        {
            //Create json token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //Retrieve security key from app settings config file
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));

            try
            {
                //Verify token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = _config["Jwt:Issuer"],
                    IssuerSigningKey = securityKey
                }, out var validatedToken);

                //Return true if token is valid
                return true;
            }
            catch
            {
                //Return false if token is not valid
                return false;
            }
        }

        /*
         * Reads given token and returns id of user
         */
        public Task<Domain.Entities.SQL.User.User> GetUserFromToken(string token)
        {
            //Create json token handler
            var handler = new JwtSecurityTokenHandler();

            //Retrieve security key from app settings config file
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));

            try
            {
                //Setup validations for token
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                //Verify token
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);


                try
                {
                    //Returns parsed token
                    return _userSqlRepository.Get(Guid.Parse(claims.Claims.ToArray()[0].Value));
                }
                catch
                {
                    // Error when parsing went wrong (value is null)
                    throw new InvalidOperationException("Something went wrong!");
                }
            }
            catch
            {
                // Error when token is invalid
                throw new InvalidTokenException();
            }
        }

        /**
         * Creates a json web token based on a given user at @param
         * Returns token as a string
         */
        public string GetJsonWebToken(Domain.Entities.SQL.User.User user)
        {
            //Retrieve security key from app settings config file
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));

            //Assign signing credentials using HmacSha256 as hashing algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Generating token using user credentials and signing credentials
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                "",
                new[]
                {
                    new Claim("ID", user.Id.ToString())

                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            //Return user json web token as string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /**
         * Authenticates given user in database
         * Returns user result from database
         */
        public async Task<Domain.Entities.SQL.User.User> AuthenticateUser(BaseUserDTO user)
        {
            // Find user in database by email
            var dbUsers = await _userSqlRepository.Filter(u => u.Email == user.Email);

            // Retrieve first user
            // Emails are always unique and so there wil never be more than 1 result
            var dbUser = dbUsers.FirstOrDefault();

            // Check if the user has been found
            if (dbUser == null)
                // Throw error if the user has not been found in the database
                throw new NotFoundException("User");

            // Check if the passwords match from given user and database user
            if (!BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password))
                // Throw error if the passwords don't match
                throw new InvalidOperationException("Credentials are incorrect!");

            // Returns user from database
            return dbUser;
        }

        /**
         * Insert user into database
         * Returns inserted user from database
         */
        public async Task<Domain.Entities.SQL.User.User> InsertUser(Domain.Entities.SQL.User.User user)
        {
            // Find user by email in database
            var emailUserResult = await _userSqlRepository.Filter(u => u.Email == user.Email);

            // Check if user exists in database
            if (emailUserResult.Any())
            {
                // Throws exception if user is already in database
                throw new DuplicateException(user.Email);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Insert user into SQL database
            await _userSqlRepository.Insert(user);

            // Saves user to database
            await _userSqlRepository.SaveChanges();

            // Find inserted user by email
            var insertedUserFound = await _userSqlRepository.Filter(u => u.Email == user.Email);

            // Insert user into neo4j database
            var insertedUser = insertedUserFound.FirstOrDefault();
            await _userNeoRepository.Insert(insertedUser);

            // Returns found user
            return insertedUser;
        }
    }
}