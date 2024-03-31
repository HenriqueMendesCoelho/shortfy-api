namespace suavesabor_api.src.User.UseCase.Exceptions
{
    public class NonAdminDemotionAttemptException : Exception
    {
        public NonAdminDemotionAttemptException() : base("Non Admin Demotion Attempt") { }
    }
}
