using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenCryptAPI.Models.Data.Post
{
    public class MutliPostModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Views { get; set; } 
    }
}
