namespace suavesabor_api.src.User.UseCase
{
    public interface IDeleteUserUseCase
    {
        Task Execute(Guid idTarget, Guid idCurrentUser);
    }
}
