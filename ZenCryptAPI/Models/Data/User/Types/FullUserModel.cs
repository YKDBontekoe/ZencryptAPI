namespace ZenCryptAPI.Models.Data.User.Types
{
    public class FullUserModel : BaseUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}