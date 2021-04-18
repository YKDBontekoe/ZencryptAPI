﻿namespace ZenCryptAPI.Models.Data.User.Types
{
    public class GeneralUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public int TotalLikes { get; set; }
        public int TotalPosts { get; set; }
        public int TotalViews { get; set; }
    }
}