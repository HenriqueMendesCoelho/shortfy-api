
using MongoDB.Bson;
using suavesabor_api.Domain.Base;

namespace suavesabor_api.Domain.User
{
    public class UserDomain : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
