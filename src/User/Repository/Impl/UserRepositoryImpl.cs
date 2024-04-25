using Microsoft.EntityFrameworkCore;
using shortfy_api.src.Application.Data;
using shortfy_api.src.Application.Repository.Generic.Impl;
using shortfy_api.User.Domain;

namespace shortfy_api.User.Repository.Impl
{
    public class UserRepositoryImpl(DataContext context) : GenericRepositoryImpl<UserDomain, Guid>(context), IUserRepository
    {
        private readonly DataContext _context = context;

        async public Task<UserDomain?> FindByEmail(string email)
        {
            return await _context.User.Include(u => u.Roles).FirstOrDefaultAsync(u => string.Equals(u.Email, email));
        }

        async public Task<UserDomain?> FindByRefreshToken(string refreshToken)
        {
            return await _context.User.Include(u => u.Roles).FirstOrDefaultAsync(u => string.Equals(u.RefreshToken, refreshToken));
        }
    }
}
