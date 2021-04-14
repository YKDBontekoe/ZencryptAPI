namespace Domain.DataTransferObjects.User
{
    public class RegisterUserDTO : BaseUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
