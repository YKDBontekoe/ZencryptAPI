using System;

namespace Domain.DataTransferObjects.Forums.Comment
{
    public class CreateCommentDTO
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Description { get; set; }
    }
}