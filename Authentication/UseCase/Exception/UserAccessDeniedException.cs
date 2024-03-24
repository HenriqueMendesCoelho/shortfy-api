namespace suavesabor_api.Authentication.UseCase.Exception
{
    public class UserAccessDeniedException : System.Exception
    {
        public UserAccessDeniedException() : base("User Access Denied") { }
    }
}
