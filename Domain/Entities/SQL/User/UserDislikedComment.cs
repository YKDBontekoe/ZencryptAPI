﻿using System;
using Domain.Entities.SQL.Forums;

namespace Domain.Entities.SQL.User
{
    public class UserDislikedComment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }

        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}