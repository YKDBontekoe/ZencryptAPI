using AutoMapper;
using Domain.DataTransferObjects.Forums.Forum;

namespace Services.Profiles
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<Domain.Entities.SQL.Forums.Forum, ForumDTO>().ReverseMap();
        }
    }
}