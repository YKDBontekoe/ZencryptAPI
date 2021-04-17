﻿using System;

namespace Domain.Entities.SQL.User
{
    public class UserViewedPost : BaseEntity
    {
        public Guid UserId { get; set; } 
        public Guid PostId { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
