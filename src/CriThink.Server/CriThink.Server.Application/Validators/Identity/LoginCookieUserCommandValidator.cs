using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class LoginCookieUserCommandValidator : AbstractValidator<LoginCookieUserCommand>
    {
        public LoginCookieUserCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .When(x => !string.IsNullOrWhiteSpace(x.Username));

            RuleFor(x => x.Username)
                .NotNull()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
