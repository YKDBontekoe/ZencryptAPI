using System;

namespace Domain.DataTransferObjects.Forums.Post
{
    public class UserInteractPostDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}