using System;

namespace Domain.DataTransferObjects.User
{
    public class TokenUserDTO
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}