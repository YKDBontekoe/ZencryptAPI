using System;

namespace Domain.DataTransferObjects.Forums.Comment.Input
{
    public class CreateCommentInput
    {
        public Guid PostId { get; set; }
        public string Description { get; set; }
    }
}