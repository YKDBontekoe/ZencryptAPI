using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Frames;
using Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.DataTransferObjects.User;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Services.User;
using ZenCryptAPI.Models.Data.User;

namespace ZenCryptAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CurPolicy")]
    public class usersController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;

        public usersController(IAuthenticationService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        // POST api/<UserController>/login
        /// <summary>
        ///     Returns a generated token from user
        /// </summary>
        /// <param name="user">Login user with email and password</param>
        /// <returns>Will return user data with token in an api frame</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] BaseUserDTO user)
        {
            try
            {
                // Authenticate user
                var rUser = await _authService.AuthenticateUser(user);

                // Map found user to LoginUserModel
                var userModel = _mapper.Map<LoginUserModel>(rUser);

                // Retrieve token from user and assign to userModel object
                userModel.Token = _authService.GetJsonWebToken(rUser);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<LoginUserModel>
                { Message = $"Welcome back {rUser.FirstName}! ", Result = userModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<UserController>/register
        /// <summary>
        ///     Creates a new user and returns new user 
        /// </summary>
        /// <param name="user">Register user with all user data</param>
        /// <returns>Will return new user in an api frame</returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] RegisterUserDTO user)
        {
            try
            {
                // Map registerDTO to User
                var mapUser = _mapper.Map<User>(user);

                // Create user
                var rUser = await _authService.InsertUser(mapUser);

                // Map found user to RegisterUserModel
                var userModel = _mapper.Map<RegisterUserModel>(rUser);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<RegisterUserModel>
                { Message = $"Welcome back {rUser.FirstName}! ", Result = userModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }
    }
}