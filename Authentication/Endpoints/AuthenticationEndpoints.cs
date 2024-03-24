using Microsoft.IdentityModel.Tokens;
using suavesabor_api.Application.Dto;
using suavesabor_api.Application.Util;
using suavesabor_api.Authentication.Endpoints.Dto;
using suavesabor_api.Authentication.Endpoints.Dto.Validators;
using suavesabor_api.Authentication.UseCase;
using suavesabor_api.Authentication.UseCase.Exception;

namespace suavesabor_api.Authentication.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void RegisterAuthenticationEndpoints(this IEndpointRouteBuilder routes)
        {
            var route = routes.MapGroup("/api/v1");

            route.MapPost("login", async (LoginRequestDto request, ILoginUseCase loginUseCase, HttpContext httpContext) =>
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
                catch (UserNotFoundException e)
                {
                    return Results.NotFound(new MessageResponseDto { Success = false, Code = 404, Message = e.Message });
                }
                catch (UserAccessDeniedException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.Problem(e.Message);
                }
            });

            route.MapPost("refresh", async (RefreshRequestDto request, IRefreshTokenUseCase refreshTokenUseCase, HttpContext httpContext) =>
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
                    return Results.Problem(e.Message);
                }
            });
        }
    }
}
