namespace suavesabor_api.src.User.UseCase.Exceptions
{
    public class UserAlreadyHasAdminPrivilegesException : Exception
    {
        public UserAlreadyHasAdminPrivilegesException() : base("User Already Has Admin Privileges") { }
    }
}
