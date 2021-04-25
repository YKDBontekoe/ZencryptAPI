using AutoMapper;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.Entities.SQL.Forums;
using Domain.Models.Comment;

namespace ZenCryptAPI.Models.Profiles.Forum
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, MultiCommentModel>()
                .ForMember(c => c.Likes, opt => opt.MapFrom(c => c.LikedByUsers.Count))
                .ForMember(c => c.Dislikes, opt => opt.MapFrom(c => c.DislikedByUsers.Count))
                .ForMember(c => c.UploadedUserName,
                    opt => opt.MapFrom(c => c.UploadedUser.UserName ?? c.UploadedUser.FirstName));

            CreateMap<Comment, SingleCommentModel>();
            CreateMap<CommentDTO, Comment>();
        }
    }
}