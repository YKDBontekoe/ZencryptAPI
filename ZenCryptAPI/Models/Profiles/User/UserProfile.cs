using AutoMapper;
using Domain.DataTransferObjects;
using ZenCryptAPI.Models.Data.User;

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
            CreateMap<Domain.Entities.User, LoginUserModel>();
            CreateMap<Domain.Entities.User, RegisterUserModel>();
            CreateMap<Domain.Entities.User, UserModel>();
            CreateMap<RegisterUserDTO, Domain.Entities.User>();
        }
    }
}