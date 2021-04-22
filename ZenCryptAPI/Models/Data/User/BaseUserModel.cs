using System;

namespace ZenCryptAPI.Models.Data.User
{
    public class BaseUserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}