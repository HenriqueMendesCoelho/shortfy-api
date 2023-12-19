using suavesabor_api.Application.Data;
using suavesabor_api.Application.Repository.Generic.Impl;
using suavesabor_api.User.Domain;

namespace suavesabor_api.User.Repository.Impl
{
    public class UserRepositoryImpl(DataContext context) : GenericRepositoryImpl<UserDomain, Guid>(context), IUserRepository
    {
    }
}
