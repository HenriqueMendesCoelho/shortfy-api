using suavesabor_api.Application.Repository.Generic;
using suavesabor_api.User.Domain;

namespace suavesabor_api.User.Repository
{
    public interface IUserRepository : IGenericRepository<UserDomain, Guid>
    {
    }
}
