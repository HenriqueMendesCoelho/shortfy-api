namespace shortfy_api.src.User.UseCase.Exceptions
{
    public class UserAcessNotAuthorizedException : Exception
    {
        public UserAcessNotAuthorizedException() : base("User Access Not Authorized") { }
    }
}
