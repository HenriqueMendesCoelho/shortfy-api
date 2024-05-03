namespace shortfy_api.src.Authentication.UseCase.Exceptions
{
    public class UserAccessDeniedException : System.Exception
    {
        public UserAccessDeniedException() : base("User Access Denied") { }

        public UserAccessDeniedException(string reason) : base($"User Access Denied reson: {reason}") { }
    }
}
