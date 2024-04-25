namespace shortfy_api.src.User.UseCase.Exceptions
{
    public class SelfDeletionNotAllowedException : Exception
    {
        public SelfDeletionNotAllowedException() : base("Self Deletion Not Allowed") { }
    }
}
