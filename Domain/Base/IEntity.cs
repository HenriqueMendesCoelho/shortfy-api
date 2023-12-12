namespace suavesabor_api.Domain.Base
{
    public interface IEntity<T>
    {
        public T? Id { get; set; }
    }
}
