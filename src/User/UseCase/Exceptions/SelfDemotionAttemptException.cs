namespace shortfy_api.src.User.UseCase.Exceptions
{
    public class SelfDemotionAttemptException : Exception
    {
        public SelfDemotionAttemptException() : base("Self Demotion Attempt") { }
    }
}
