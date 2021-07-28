using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    internal class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty();
        }
    }
}
