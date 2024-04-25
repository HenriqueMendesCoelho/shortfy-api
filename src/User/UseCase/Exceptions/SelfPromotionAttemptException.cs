namespace shortfy_api.src.User.UseCase.Exceptions
{
    public class SelfPromotionAttemptException : Exception
    {
        public SelfPromotionAttemptException() : base("Self Promotion Attempt") { }
    }
}
