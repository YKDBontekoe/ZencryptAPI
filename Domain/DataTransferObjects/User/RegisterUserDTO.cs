namespace Domain.DataTransferObjects.User
{
    public class RegisterUserDTO : BaseUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
