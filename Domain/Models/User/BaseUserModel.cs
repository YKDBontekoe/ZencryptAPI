using System;

namespace Domain.Models.User
{
    public class BaseUserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}