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
            CreateMap<Post, MultiPostModel>()
                .ForMember(t => t.Likes, opt => opt.MapFrom(d => d.LikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Dislikes, opt => opt.MapFrom(d => d.DislikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Views, opt => opt.MapFrom(d => d.ViewedByUsers.Count(c => c.IsActive))); 

            CreateMap<Post, SinglePostModel>()
                .ForMember(t => t.Likes, opt => opt.MapFrom(d => d.LikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Dislikes, opt => opt.MapFrom(d => d.DislikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Views, opt => opt.MapFrom(d => d.ViewedByUsers.Count(c => c.IsActive)));

            CreateMap<CreatePostDTO, Post>();
            CreateMap<UpdatePostDTO, Post>();
        }
    }
}
