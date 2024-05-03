using shortfy_api.src.Application.Util;
using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.User.Repository;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class LoginUseCaseImpl(IUserRepository userRepository,
        ICreateUserLoginUseCase createUserLoginUseCase) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICreateUserLoginUseCase _createUserLoginUseCase = createUserLoginUseCase;

        async public Task<LoginDomain> ExecuteAsync(string email, string passwordString)
        {
            var user = await _userRepository.FindByEmail(email) ?? throw new UserNotFoundException();
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new UserAccessDeniedException();
            }
            if (PasswordHasherUtil.VerifyPassword(passwordString, user.Password) is not true)
            {
                throw new UserAccessDeniedException();
            }

            return await _createUserLoginUseCase.ExecuteAsync(user);
        }
    }
}
