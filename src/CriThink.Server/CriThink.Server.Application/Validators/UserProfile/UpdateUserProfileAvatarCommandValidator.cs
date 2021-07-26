using CriThink.Server.Application.Commands;
using FluentValidation;

namespace CriThink.Server.Application.Validators.UserProfile
{
    internal class UpdateUserProfileAvatarCommandValidator : AbstractValidator<UpdateUserProfileAvatarCommand>
    {
        public UpdateUserProfileAvatarCommandValidator()
        {
            RuleFor(x => x.FormFile)
                .NotNull();
        }
    }
}
