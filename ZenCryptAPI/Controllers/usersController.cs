using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Frames;
using Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZenCryptAPI.Models.User;

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

        // POST api/<UserController>
        /// <summary>
        ///     Returns a generated token from user
        /// </summary>
        /// <param name="user">Login user with email and password</param>
        /// <returns>Will return user data with token in an api frame</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
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
    }
}