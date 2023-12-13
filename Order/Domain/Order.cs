using MongoDB.Bson;
using suavesabor_api.Application.Domain.Base;

namespace suavesabor_api.Order.Domain
{
    public class Order : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
    }
}
