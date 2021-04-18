using System;

namespace ZenCryptAPI.Models.Data.User
{
    public class LoginUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}