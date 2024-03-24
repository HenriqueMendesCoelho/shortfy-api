namespace suavesabor_api.Authentication.UseCase.Exception
{
    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
