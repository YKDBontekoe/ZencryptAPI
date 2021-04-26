using AutoMapper;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.SQL.Forums;

namespace Services.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(c => c.Likes, opt => opt.MapFrom(c => c.LikedByUsers.Count))
                .ForMember(c => c.Dislikes, opt => opt.MapFrom(c => c.DislikedByUsers.Count))
                .ForMember(c => c.Views, opt => opt.MapFrom(c => c.ViewedByUsers.Count))
                .PreserveReferences().ReverseMap();
        }
    }
}