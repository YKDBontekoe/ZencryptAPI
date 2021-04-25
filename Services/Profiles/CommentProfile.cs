using AutoMapper;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.Entities.SQL.Forums;

namespace Services.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(c => c.Likes, opt => opt.MapFrom(c => c.LikedByUsers.Count))
                .ForMember(c => c.Dislikes, opt => opt.MapFrom(c => c.DislikedByUsers.Count))
                .ForMember(c => c.UploadedBy, opt => opt.MapFrom(c => c.UploadedUser))
                .PreserveReferences().ReverseMap();
            
        }
    }
}