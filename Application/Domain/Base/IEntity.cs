namespace suavesabor_api.Application.Domain.Base
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
