using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators.UserProfile
{
    internal class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("Provide valid value for UserId");
        }
    }
}
