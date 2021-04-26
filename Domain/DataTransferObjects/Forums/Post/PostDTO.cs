using System;
using System.Collections.Generic;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Forums.Post
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Views { get; set; }
        public InfoUserDTO UploadedByUser { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}