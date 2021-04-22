using System.Linq;
using AutoMapper;
using Domain.DataTransferObjects.User;
using Domain.Types.User;
using ZenCryptAPI.Models.Data.User;
using ZenCryptAPI.Models.Data.User.Types;

namespace ZenCryptAPI.Models.Profiles.User
{
    /// <summary>
    ///     Maps user with models
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        ///     Maps user with Login User Model and User Model
        /// </summary>
        public UserProfile()
        {
            CreateMap<Domain.Entities.SQL.User.User, LoginUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, BaseUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, RegisterUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, GeneralUserModel>()
                .ForMember(c => c.TotalLikes, opt => opt.MapFrom(c => c.UploadedPosts.Sum(t => t.LikedByUsers.Count)))
                .ForMember(c => c.TotalPosts, opt => opt.MapFrom(c => c.UploadedPosts.Count))
                .ForMember(c => c.TotalViews, opt => opt.MapFrom(c => c.UploadedPosts.Sum(t => t.ViewedByUsers.Count)))
                .ForMember(c => c.TotalCommments, opt => opt.MapFrom(c => c.UploadedComments.Count));

            CreateMap<Domain.Entities.SQL.User.User, MinimalUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, FullUserModel>();
            CreateMap<ProfileUser, ProfileUserModel>();
            CreateMap<RegisterUserDTO, Domain.Entities.SQL.User.User>();
        }
    }
}