using suavesabor_api.src.Application.Domain.Base;

namespace suavesabor_api.src.Order.Domain
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
