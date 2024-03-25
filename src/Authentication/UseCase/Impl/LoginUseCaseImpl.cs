﻿using suavesabor_api.src.Application.Util;
using suavesabor_api.src.Authentication.Domain;
using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.User.Repository;

namespace suavesabor_api.src.Authentication.UseCase.Impl
{
    public class LoginUseCaseImpl(IUserRepository userRepository, ICreateRefreshTokenUseCase createRefreshTokenUseCase,
        ICreateTokenUseCase createTokenUseCase) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICreateRefreshTokenUseCase _createRefreshTokenUseCase = createRefreshTokenUseCase;
        private readonly ICreateTokenUseCase _createTokenUseCase = createTokenUseCase;

        async public Task<LoginDomain> Execute(string email, string passwordString)
        {
            var user = await _userRepository.FindByEmail(email) ?? throw new UserNotFoundException();

            if (PasswordHasherUtil.VerifyPassword(passwordString, user.Password) is not true) throw new UserAccessDeniedException();

            var newRefreshToken = _createRefreshTokenUseCase.Execute();
            var roles = user.Roles.Select(r => r.ToString() ?? throw new UserAccessDeniedException()).ToList();
            var acessToken = _createTokenUseCase.Execute(user.Id, user.Email, roles);

            user.RefreshToken = newRefreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = newRefreshToken.Expiration;
            await _userRepository.Update(user);

            return new LoginDomain { Authenticated = true, AccessToken = acessToken.AccessToken, ExpirationAcessToken = acessToken.Expiration, RefreshToken = newRefreshToken.RefreshToken, ExpirationRefreshToken = newRefreshToken.Expiration };
        }
    }
}