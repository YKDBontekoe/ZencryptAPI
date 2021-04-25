namespace Domain.Models.User
{
    public class RegisterUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}