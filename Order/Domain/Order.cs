using suavesabor_api.Application.Domain.Base;

namespace suavesabor_api.Order.Domain
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
