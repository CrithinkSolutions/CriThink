using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators.Identity
{
    internal class CreateNewUserAsAdminCommandValidator : AbstractValidator<CreateNewUserAsAdminCommand>
    {
        public CreateNewUserAsAdminCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Username)
                .NotEmpty();
        }
    }
}
