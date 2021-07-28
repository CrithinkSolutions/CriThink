using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class ExchangeRefreshTokenCommandValidator : AbstractValidator<ExchangeRefreshTokenCommand>
    {
        public ExchangeRefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotNull();

            RuleFor(x => x.RefreshToken)
                .NotNull();
        }
    }
}
