using System;

namespace Domain.DataTransferObjects.Forums.Post
{
    public class CreatePostDTO
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}