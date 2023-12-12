using MongoDB.Bson;
using suavesabor_api.Data;
using suavesabor_api.Domain.User;
using suavesabor_api.Repository.Generic;
using suavesabor_api.Repository.Generic.Impl;

namespace suavesabor_api.Repository.Impl
{
    public class UserRepositoryImpl(DataContext context) : GenericRepositoryImpl<UserDomain, ObjectId>(context), IUserRepository
    {
    }
}
