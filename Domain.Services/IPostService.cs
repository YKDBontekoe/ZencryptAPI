using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums;
using Domain.Entities.Forums;

namespace Domain.Services
{
    public interface IPostService
    {
        Task<Post> CreatePost(Post post, string token);
        Task<Post> UpdatePost(Post post, string token);
        Task<Post> DeletePost(Guid postId, string token);
        Task<Post> GetPost(Guid postId);
        Task<IEnumerable<Post>> GetPosts();
    }
}
