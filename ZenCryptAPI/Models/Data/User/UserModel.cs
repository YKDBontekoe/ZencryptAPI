using System;

namespace ZenCryptAPI.Models.Data.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int TotalLikes { get; set; }
        public int TotalPosts { get; set; }
        public int TotalViews { get; set; }
    }
}