using System;

namespace Domain.DataTransferObjects.User
{
    public class InfoUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int CommentsPlaced { get; set; }
        public int PostsPlaced { get; set; }
        public int Viewed { get; set; }
        public int LikesGivenToPosts { get; set; }
        public int LikesGivenToComments { get; set; }
        public int DislikesGivenToPosts { get; set; }
        public int DislikesGivenToComments { get; set; }
    }
}