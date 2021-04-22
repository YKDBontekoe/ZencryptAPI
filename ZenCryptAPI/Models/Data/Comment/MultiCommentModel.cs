using System;

namespace ZenCryptAPI.Models.Data.Comment
{
    public class MultiCommentModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public string UploadedUserName { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}