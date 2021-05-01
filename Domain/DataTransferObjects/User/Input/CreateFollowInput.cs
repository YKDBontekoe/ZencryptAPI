using System;

namespace Domain.DataTransferObjects.User.Input
{
    public class CreateFollowInput
    {
        public Guid UserToFollowId { get; set; }
    }
}