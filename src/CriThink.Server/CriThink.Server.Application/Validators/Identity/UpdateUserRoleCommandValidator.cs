using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators
{
    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Role)
                .NotEmpty();
        }
    }
}
