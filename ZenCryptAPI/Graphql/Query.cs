using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Post;
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
        public async Task<IQueryable<PostDTO>> GetPosts([Service] IPostService postService)
        {
            return await postService.GetPosts() as IQueryable<PostDTO>;
        }
    }
}