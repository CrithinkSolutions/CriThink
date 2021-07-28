using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class VerifyAccountEmailCommandValidator : AbstractValidator<VerifyAccountEmailCommand>
    {
        public VerifyAccountEmailCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotNull();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
