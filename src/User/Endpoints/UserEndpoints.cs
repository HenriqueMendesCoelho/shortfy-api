using suavesabor_api.src.Application.Data;
using suavesabor_api.src.Application.Util;
using suavesabor_api.User.Endpoints.Dto;
using suavesabor_api.User.Endpoints.Dto.Validators;
using suavesabor_api.User.UseCase;

namespace suavesabor_api.User.Endpoints
{
    public static class UserEndpoints
    {

        public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var users = routes.MapGroup("/api/v1/user");

            users.MapGet("", async (ISearchUserUseCase useCase, HttpContext httpContext) =>
            {
                if (!httpContext.User?.Identity?.IsAuthenticated is true)
                {
                    return Results.Unauthorized();
                }

                List<UserResponseDto> response = (await useCase.ListAll()).Select(user => new UserResponseDto(user)).ToList();
                if (response.Count() == 0)
                {
                    Results.NoContent();
                }

                return Results.Ok(response);
            });

            users.MapPost("", async (UserRequestDto request, ICreateUserUseCase useCase, DataContext context) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new UserRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }
                UserResponseDto response = new(await useCase.Create(request.ToDomain()));

                return Results.Ok(response);
            });
        }
    }
}
