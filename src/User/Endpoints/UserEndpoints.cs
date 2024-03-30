using suavesabor_api.src.Application.Dto;
using suavesabor_api.src.Application.Exceptions;
using suavesabor_api.src.Application.Util;
using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.src.User.Endpoints.Dto;
using suavesabor_api.src.User.Endpoints.Dto.Validators;
using suavesabor_api.src.User.UseCase;
using suavesabor_api.src.User.UseCase.Exceptions;
using suavesabor_api.User.Endpoints.Dto;
using suavesabor_api.User.Endpoints.Dto.Validators;
using suavesabor_api.User.UseCase;
using System.Security.Claims;

namespace suavesabor_api.User.Endpoints
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var users = routes.MapGroup("/api/v1/user");

            users.MapGet("", async (ISearchUserUseCase useCase) =>
            {
                List<UserResponseDto> response = (await useCase.ListAll()).Select(user => new UserResponseDto(user)).ToList();
                if (response is null || response.Count == 0)
                {
                    Results.NoContent();
                }

                return Results.Ok(response);
            }).RequireAuthorization("ADMIN");

            users.MapGet("/{id}", async (Guid id, ISearchUserUseCase useCase) =>
            {
                try
                {
                    var response = new UserResponseDto(await useCase.FindByID(id));
                    return Results.Ok(response);
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound(MessageResponseDto.Create("User not found", 404));
                }
            });

            users.MapPost("", async (UserRequestDto request, ICreateUserUseCase useCase, ClaimsPrincipal userClaims,
                ILogger logger) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new UserRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }

                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetIdNullable(userClaims);
                    UserResponseDto response = new(await useCase.Execute(request.ToDomain(), idCurrentUser));

                    return Results.Ok(response);
                }
                catch (EmailConflictException e)
                {
                    return Results.Problem(e.Message, statusCode: 409);
                }
                catch (UserAccessDeniedException)
                {
                    return Results.Forbid();
                }
                catch (UserAcessNotAuthorizedException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on update user '(POST)/api/v1/user'");
                    return Results.Problem("Internal Server Error, contact administrator");
                }
            });

            users.MapPut("/{id}", async (Guid id, UserUpdateRequestDto request, IUpdateUserUseCase useCase, ClaimsPrincipal userClaims,
                ILogger logger) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new UserUpdateRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }

                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetId(userClaims);
                    UserResponseDto response = new(await useCase.Execute(request.ToDomain(), id, idCurrentUser));
                    return Results.Ok(response);
                }
                catch (TokenNotValidExpcetion)
                {
                    return Results.Unauthorized();
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound(MessageResponseDto.Create("User not found", 404));
                }
                catch (UserAccessDeniedException)
                {
                    return Results.Problem("Access denied, can update only yourself", statusCode: 403);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on update user '(PUT)/api/v1/user/${id}'");
                    return Results.Problem("Internal Server Error, contact administrator");
                }

            }).RequireAuthorization("USER");

            users.MapDelete("/{id}", async (Guid id, IDeleteUserUseCase useCase, ClaimsPrincipal userClaims, ILogger logger) =>
            {
                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetId(userClaims);
                    await useCase.Execute(id, idCurrentUser);
                    return Results.NoContent();
                }
                catch (TokenNotValidExpcetion)
                {
                    return Results.Unauthorized();
                }
                catch (SelfDeletionNotAllowedException e)
                {
                    return Results.Problem(e.Message, statusCode: 400);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on delete user '(DELETE)/api/v1/user/${id}'");
                    return Results.Problem("Internal Server Error, contact administrator");
                }
            }).RequireAuthorization("ADMIN");
        }
    }
}
