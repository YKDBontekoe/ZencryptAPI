﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenCryptAPI.Models.Data.Comment;
using ZenCryptAPI.Models.Data.User.Types;

namespace ZenCryptAPI.Models.Data.Post
{
    public class SinglePostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public MinimalUserModel UploadedBy { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Views { get; set; }
    }
}
