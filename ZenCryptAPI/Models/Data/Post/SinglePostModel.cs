using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenCryptAPI.Models.Data.Comment;

namespace ZenCryptAPI.Models.Data.Post
{
    public class SinglePostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string UploadedBy { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Views { get; set; }

        public IEnumerable<MultiCommentModel> Comments { get; set; }
    }
}
