using System;

namespace Domain.DataTransferObjects.User
{
    public class SafeUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
