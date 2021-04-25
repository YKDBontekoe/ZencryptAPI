using AutoMapper;
using Domain.DataTransferObjects.User;

namespace Services.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SafeUserDTO, Domain.Entities.SQL.User.User>().ReverseMap();
        }
    }
}