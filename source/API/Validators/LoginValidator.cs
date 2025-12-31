using FluentValidation;
using SocialWorkApi.API.Dto.Auth;

namespace SocialWorkApi.API.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(login => login.Email).NotNull();
        RuleFor(login => login.Password).NotNull();
    }
}