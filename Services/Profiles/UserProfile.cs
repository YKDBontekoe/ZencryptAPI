using System.Linq;
using AutoMapper;
using Domain.DataTransferObjects.User;

namespace Services.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SafeUserDTO, Domain.Entities.SQL.User.User>().ReverseMap();
            CreateMap<Domain.Entities.SQL.User.User, InfoUserDTO>()
                .ForMember(c => c.Viewed, opt => opt.MapFrom(c => c.ViewedPosts.Count(c => c.IsActive)))
                .ForMember(c => c.LikesGivenToComments, opt => opt.MapFrom(c => c.LikedComments.Count(c => c.IsActive)))
                .ForMember(c => c.LikesGivenToPosts, opt => opt.MapFrom(c => c.LikedPosts.Count(c => c.IsActive)))
                .ForMember(c => c.DislikesGivenToComments,
                    opt => opt.MapFrom(c => c.DislikedComments.Count(c => c.IsActive)))
                .ForMember(c => c.DislikesGivenToPosts, opt => opt.MapFrom(c => c.DislikedPosts.Count(c => c.IsActive)))
                .ForMember(c => c.CommentsPlaced, opt => opt.MapFrom(c => c.UploadedPosts.Count(c => c.IsActive)))
                .ForMember(c => c.PostsPlaced, opt => opt.MapFrom(c => c.UploadedComments.Count(c => c.IsActive)))
                .ReverseMap();
        }
    }
}