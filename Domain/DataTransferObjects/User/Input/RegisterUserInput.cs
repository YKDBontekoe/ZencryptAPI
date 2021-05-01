namespace Domain.DataTransferObjects.User.Input
{
    public class RegisterUserInput : LoginUserInput
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}