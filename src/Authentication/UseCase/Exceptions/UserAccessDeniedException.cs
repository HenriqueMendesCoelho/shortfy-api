namespace shortfy_api.src.Authentication.UseCase.Exceptions
{
    public class UserAccessDeniedException : System.Exception
    {
        public UserAccessDeniedException() : base("User Access Denied") { }
    }
}
