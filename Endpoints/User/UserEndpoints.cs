using Microsoft.AspNetCore.Mvc;
using suavesabor_api.Data;
using suavesabor_api.Domain.User;
using suavesabor_api.Endpoints.User.Dto;
using suavesabor_api.UseCase.User;

namespace suavesabor_api.Endpoints.User
{
    public static class UserEndpoints
    {

        public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var users = routes.MapGroup("/api/v1/user");

            users.MapGet("", (ISearchUserUseCase useCase) =>
            {
                List<UserDomain> usersList = useCase.listAll();
                return usersList.Select(user => new UserResponseDto(user)).ToList();
            });

            users.MapPost("", (UserRequestDto request, DataContext context) =>
            {
                context.UserDomain.Add(request.ToDomain());
                context.SaveChanges();
            });
        }
    }
}
