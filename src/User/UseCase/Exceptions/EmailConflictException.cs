namespace shortfy_api.src.User.UseCase.Exceptions
{
    public class EmailConflictException : Exception
    {
        public EmailConflictException() : base("Email Conflict") { }
    }
}
