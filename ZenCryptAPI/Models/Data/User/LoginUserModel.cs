using System;

namespace ZenCryptAPI.Models.Data.User
{
    public class LoginUserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}