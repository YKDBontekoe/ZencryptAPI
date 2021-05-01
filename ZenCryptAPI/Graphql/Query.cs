using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Forum;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.Forums.Post.Input;
using Domain.Services.Forum;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace ZenCryptAPI.Graphql
{
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseOffsetPaging]
        public async Task<IQueryable<PostDTO>> GetPosts([Service] IPostService postService, Guid? forumdId)
        {
            return await postService.GetPosts(forumdId) as IQueryable<PostDTO>;
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        [UseOffsetPaging]
        public async Task<IQueryable<ForumDTO>> GetForums([Service] IForumService forumService)
        {
            return await forumService.GetForums() as IQueryable<ForumDTO>;
        }
    }
}