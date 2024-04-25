using shortfy_api.src.Application.Domain.Base;

namespace shortfy_api.src.Order.Domain
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
