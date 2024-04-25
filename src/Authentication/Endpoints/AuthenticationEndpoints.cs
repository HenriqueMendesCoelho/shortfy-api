using Microsoft.IdentityModel.Tokens;
using shortfy_api.src.Application.Dto;
using shortfy_api.src.Application.Util;
using shortfy_api.src.Authentication.Endpoints.Dto;
using shortfy_api.src.Authentication.Endpoints.Dto.Validators;
using shortfy_api.src.Authentication.UseCase;
using shortfy_api.src.Authentication.UseCase.Exceptions;

namespace shortfy_api.src.Authentication.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void RegisterAuthenticationEndpoints(this IEndpointRouteBuilder routes)
        {
            var route = routes.MapGroup("/api/v1");

            route.MapPost("/google-signin", async (GoogleLoginRequestDto request, ILoginUseCase loginUseCase, ILogger logger) =>
            {
                try
                {
                    if (request is null || string.IsNullOrEmpty(request.idToken))
                    {
                        return Results.BadRequest();
                    }
                    var token = request.idToken!;
                    var response = await loginUseCase.ExecuteFromGoogle(token);

                    return Results.Ok(response);
                }
                catch (Exception e) when (e is UserNotFoundException || e is UserAccessDeniedException)
                {
                    logger.LogError(e.Message, e);
                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, e);
                    return Results.Problem(e.Message);
                }
            });

            route.MapPost("login", async (LoginRequestDto request, ILoginUseCase loginUseCase, HttpContext httpContext, ILogger logger) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new LoginRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }

                try
                {
                    var response = await loginUseCase.Execute(request.Email, request.Password);
                    return Results.Ok(response);
                }
                catch (Exception e) when (e is UserNotFoundException || e is UserAccessDeniedException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, e);
                    return Results.Problem(e.Message);
                }
            });

            route.MapPost("refresh", async (RefreshRequestDto request, IRefreshTokenUseCase refreshTokenUseCase, HttpContext httpContext, ILogger logger) =>
            {
                var validationResult = ValidationRequestUtil.IsValid(new RefreshTokenRequestDtoValidator(), request);
                if (validationResult is not true)
                {
                    return Results.BadRequest(validationResult);
                }

                try
                {
                    var response = await refreshTokenUseCase.Execute(request.RefreshToken);
                    return Results.Ok(response);
                }
                catch (UserNotFoundException e)
                {
                    return Results.NotFound(new MessageResponseDto { Success = false, Code = 404, Message = e.Message });
                }
                catch (SecurityTokenException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message, e);
                    return Results.Problem(e.Message);
                }
            });
        }
    }
}
