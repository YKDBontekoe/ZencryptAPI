using AutoMapper;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
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
            CreateMap<Domain.Entities.SQL.User.User, LoginUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, RegisterUserModel>();
            CreateMap<Domain.Entities.SQL.User.User, UserModel>();
            CreateMap<RegisterUserDTO, Domain.Entities.SQL.User.User>();
        }
    }
}