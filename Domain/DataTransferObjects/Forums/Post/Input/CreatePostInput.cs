﻿using System;

namespace Domain.DataTransferObjects.Forums.Post.Input
{
    public class CreatePostInput
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid ForumId { get; set; }
    }
}