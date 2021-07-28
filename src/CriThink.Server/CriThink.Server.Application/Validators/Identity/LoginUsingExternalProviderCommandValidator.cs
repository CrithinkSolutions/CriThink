using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class LoginUsingExternalProviderCommandValidator : AbstractValidator<LoginJwtUsingExternalProviderCommand>
    {
        public LoginUsingExternalProviderCommandValidator()
        {
            RuleFor(x => x.SocialProvider)
                .NotEmpty();

            RuleFor(x => x.UserToken)
                .NotEmpty();
        }
    }
}
