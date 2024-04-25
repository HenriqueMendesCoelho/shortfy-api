namespace shortfy_api.src.Authentication.UseCase.Exceptions
{
    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
