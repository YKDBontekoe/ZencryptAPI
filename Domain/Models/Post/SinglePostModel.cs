using System;
using Domain.Models.User.Types;

namespace Domain.Models.Post
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