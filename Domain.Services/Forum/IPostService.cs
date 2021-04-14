﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Forums;

namespace Domain.Services.Forum
{
    public interface IPostService
    {
        Task<Post> CreatePost(Post post, string token);
        Task<Post> UpdatePost(Guid postId, Post post, string token);
        Task<Post> DeletePost(Guid postId, string token); 
        Task<Post> GetPost(Guid postId);
        Task<IEnumerable<Post>> GetPosts();
    }
}
