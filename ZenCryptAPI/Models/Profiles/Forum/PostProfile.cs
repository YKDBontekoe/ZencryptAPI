using System.Linq;
using AutoMapper;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.SQL.Forums;
using Domain.Models.Post;

namespace ZenCryptAPI.Models.Profiles.Forum
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, MultiPostModel>()
                .ForMember(t => t.Likes, opt => opt.MapFrom(d => d.LikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Dislikes, opt => opt.MapFrom(d => d.DislikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Views, opt => opt.MapFrom(d => d.ViewedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.UploadedBy,
                    opt => opt.MapFrom(d => d.UploadedByUser.UserName ?? d.UploadedByUser.FirstName));

            CreateMap<Post, SinglePostModel>()
                .ForMember(t => t.Likes, opt => opt.MapFrom(d => d.LikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Dislikes, opt => opt.MapFrom(d => d.DislikedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.Views, opt => opt.MapFrom(d => d.ViewedByUsers.Count(c => c.IsActive)))
                .ForMember(t => t.UploadedBy, opt => opt.MapFrom(d => d.UploadedByUser));

            CreateMap<CreatePostDTO, Post>();
            CreateMap<UpdatePostDTO, Post>();
        }
    }
}