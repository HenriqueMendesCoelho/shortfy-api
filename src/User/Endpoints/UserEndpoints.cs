using Microsoft.AspNetCore.Mvc;
using shortfy_api.src.Application.Domain;
using shortfy_api.src.Application.Dto;
using shortfy_api.src.Application.Exceptions;
using shortfy_api.src.Application.Util;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.src.User.Endpoints.Dto;
using shortfy_api.src.User.Endpoints.Dto.Validators;
using shortfy_api.src.User.UseCase;
using shortfy_api.src.User.UseCase.Exceptions;
using shortfy_api.User.Endpoints.Dto;
using shortfy_api.User.Endpoints.Dto.Validators;
using shortfy_api.User.UseCase;
using System.Security.Claims;

namespace shortfy_api.User.Endpoints
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
            }).RequireAuthorization("ADMIN").Produces<List<UserResponseDto>>(200);

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
            }).RequireAuthorization().Produces<UserResponseDto>(200);

            users.MapPost("", async (UserRequestDto request, ICreateUserUseCase useCase, ILogger logger) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new UserRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }

                try
                {
                    UserResponseDto response = new(await useCase.Execute(request.ToDomain()));

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
            }).Produces<UserResponseDto>(200);

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

            }).RequireAuthorization("USER").Produces<UserResponseDto>(200);

            users.MapPatch("/{id}/roles/promote", async (Guid id, IPromoteUserUseCase useCase, ClaimsPrincipal userClaims, ILogger logger) =>
            {
                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetId(userClaims);
                    MessageDomain domain = await useCase.Execute(id, idCurrentUser);
                    return Results.Ok(new MessageResponseDto(domain));
                }
                catch (Exception e) when (e is UserAlreadyHasAdminPrivilegesException || e is SelfPromotionAttemptException || e is UserNotFoundException)
                {
                    return Results.BadRequest(MessageResponseDto.Create(e.Message, 400));
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on update user '(PATCH)/api/v1/user/${id}/roles/promote'");
                    return Results.Problem("Internal Server Error, contact administrator");
                }

            }).RequireAuthorization("ADMIN").Produces<MessageDomain>(200);

            users.MapPatch("/{id}/roles/demote", async (Guid id, IDemoteUserUseCase useCase, ClaimsPrincipal userClaims, ILogger logger) =>
            {
                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetId(userClaims);
                    MessageDomain domain = await useCase.Execute(id, idCurrentUser);
                    return Results.Ok(new MessageResponseDto(domain));
                }
                catch (Exception e) when (e is NonAdminDemotionAttemptException || e is SelfDemotionAttemptException || e is UserNotFoundException)
                {
                    return Results.BadRequest(MessageResponseDto.Create(e.Message, 400));
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on update user '(PATCH)/api/v1/user/${id}/roles/demote'");
                    return Results.Problem("Internal Server Error, contact administrator");
                }

            }).RequireAuthorization("ADMIN")
            .Produces<MessageDomain>(200);

            users.MapDelete("/{id}", async (Guid id, IDeleteUserUseCase useCase, ClaimsPrincipal userClaims, ILogger logger) =>
            {
                try
                {
                    var idCurrentUser = UserClaimsPrincipalUtil.GetId(userClaims);
                    await useCase.Execute(id, idCurrentUser);
                    return TypedResults.NoContent();
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
            }).RequireAuthorization("ADMIN")
            .Produces(204)
            .Produces<ProblemDetails>(400)
            .Produces(401)
            .Produces<ProblemDetails>(500);

        }
    }
}
