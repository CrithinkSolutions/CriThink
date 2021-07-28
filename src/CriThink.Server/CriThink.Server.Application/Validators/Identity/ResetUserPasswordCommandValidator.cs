using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
    {
        public ResetUserPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
