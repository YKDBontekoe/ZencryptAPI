using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.User;
using Domain.Entities;
using Domain.Entities.SQL.Forums;
using Domain.Entities.SQL.User;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace Infrastructure.EF.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<PostDTO>> GetPosts([Service]IPostService postService) =>
            await postService.GetPosts() as IQueryable<PostDTO>;
    }
}