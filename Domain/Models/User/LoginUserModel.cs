namespace Domain.Models.User
{
    public class LoginUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}