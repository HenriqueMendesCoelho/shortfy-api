using FluentValidation;
using FluentValidation.Results;

namespace suavesabor_api.src.Application.Util
{
    public class ValidationRequestUtil
    {
        public static object IsValid<T>(AbstractValidator<T> validator, T instanceToValidate)
        {
            ValidationResult result = validator.Validate(instanceToValidate);
            if (!result.IsValid)
            {
                return result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            }

            return true;
        }
    }
}
