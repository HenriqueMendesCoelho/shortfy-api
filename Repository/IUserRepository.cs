using MongoDB.Bson;
using suavesabor_api.Domain.User;
using suavesabor_api.Repository.Generic;

namespace suavesabor_api.Repository
{
    public interface IUserRepository : IGenericRepository<UserDomain, ObjectId>
    {
    }
}
