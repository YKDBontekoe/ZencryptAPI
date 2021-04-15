using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DataTransferObjects.Forums;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.Forums;
using ZenCryptAPI.Models.Data.Post;

namespace ZenCryptAPI.Models.Profiles.Forum
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, MultiPostModel>();
            CreateMap<Post, SinglePostModel>();
            CreateMap<CreatePostDTO, Post>();
            CreateMap<UpdatePostDTO, Post>();
        }
    }
}
