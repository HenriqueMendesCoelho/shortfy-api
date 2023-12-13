
using MongoDB.Bson;
using suavesabor_api.Application.Attributes;
using suavesabor_api.Application.Domain.Base;

namespace suavesabor_api.User.Domain
{
    //[IncludeRelatedEntities("Orders", "Addresses")]
    public class UserDomain : IEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
