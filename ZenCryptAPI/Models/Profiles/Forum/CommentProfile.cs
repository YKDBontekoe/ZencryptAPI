using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DataTransferObjects.Forums;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.SQL.Forums;
using ZenCryptAPI.Models.Data.Comment;
using ZenCryptAPI.Models.Data.Post;

namespace ZenCryptAPI.Models.Profiles.Forum
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, MultiCommentModel>();
            CreateMap<Comment, SingleCommentModel>();
            CreateMap<CommentDTO, Comment>();
        }
    }
}
