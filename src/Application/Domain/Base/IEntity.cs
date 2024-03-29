namespace suavesabor_api.src.Application.Domain.Base
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
