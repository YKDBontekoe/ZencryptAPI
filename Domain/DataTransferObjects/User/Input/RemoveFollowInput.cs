using System;

namespace Domain.DataTransferObjects.User.Input
{
    public class RemoveFollowInput
    {
        public Guid UserToFollowId { get; set; }
    }
}