namespace ZenCryptAPI.Models.User
{
    public class LoginUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsDepartmentLead { get; set; }
    }
}