using System;
using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Forums.Comment
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public SafeUserDTO UploadedBy { get; set; }
    }
}