using System.Linq;
using AutoMapper;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
using Domain.Types.User;
using ZenCryptAPI.Models.Data.User;
using ZenCryptAPI.Models.Data.User.Types;


namespace ZenCryptAPI.Models.Profiles.User
{
    /// <summary>
    /// Maps user with models
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Maps user with Login User Model and User Model
        /// </summary>
        public UserProfile()
        {
            CreateMap<Domain.Entities.SQL.User.User, LoginUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, BaseUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, RegisterUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, GeneralUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, MinimalUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, FullUserModel>();
            CreateMap<ProfileUser, ProfileUserModel>();
            CreateMap<RegisterUserDTO, Domain.Entities.SQL.User.User>();
        }
    }
}