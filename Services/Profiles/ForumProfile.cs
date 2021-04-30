using System.Linq;
using AutoMapper;
using Domain.DataTransferObjects.Forums.Forum;

namespace Services.Profiles
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<Domain.Entities.SQL.Forums.Forum, ForumDTO>()
                .ForMember(c => c.TotalPosts, opt => opt.MapFrom(c => c.Posts.Count))
                 .ForMember(c => c.TotalComments, opt => opt.MapFrom(c => c.Posts.Where(x => x.Comments.Any()).SelectMany(x => x.Comments).Count(c => c.IsActive)))
                 .ForMember(c => c.TotalLikes, opt => opt.MapFrom(c => c.Posts.Where(x => x.LikedByUsers.Any()).SelectMany(x => x.LikedByUsers).Count(c => c.IsActive)))
                 .ForMember(c => c.TotalDislikes, opt => opt.MapFrom(c => c.Posts.Where(x => x.DislikedByUsers.Any()).SelectMany(x => x.DislikedByUsers).Count(c => c.IsActive)))
                 .ForMember(c => c.TotalViews, opt => opt.MapFrom(c => c.Posts.Where(x => x.ViewedByUsers.Any()).SelectMany(x => x.ViewedByUsers).Count(c => c.IsActive)))
                .ReverseMap();
        }
    }
}