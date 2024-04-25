using shortfy_api.src.Application.Repository.Generic;
using shortfy_api.User.Domain;

namespace shortfy_api.User.Repository
{
    public interface IUserRepository : IGenericRepository<UserDomain, Guid>
    {

        Task<UserDomain?> FindByEmail(string email);

        Task<UserDomain?> FindByRefreshToken(string refreshToken);
    }
}
