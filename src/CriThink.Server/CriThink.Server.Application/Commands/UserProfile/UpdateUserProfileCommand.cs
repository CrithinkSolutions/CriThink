using System;
using CriThink.Server.Domain.Entities;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Update user profile information
    /// </summary>
    public class UpdateUserProfileCommand : IRequest
    {
        public UpdateUserProfileCommand(
            Guid userId,
            string givenName,
            string familyName,
            string description,
            Gender? gender,
            string country,
            string telegram,
            string skype,
            string twitter,
            string instagram,
            string facebook,
            string snapchat,
            string youtube,
            string blog,
            DateTime? dateOfBirth)
        {
            UserId = userId;
            GivenName = givenName;
            FamilyName = familyName;
            Description = description;
            Gender = gender;
            Country = country;
            Telegram = telegram;
            Skype = skype;
            Twitter = twitter;
            Instagram = instagram;
            Facebook = facebook;
            Snapchat = snapchat;
            Youtube = youtube;
            Blog = blog;
            DateOfBirth = dateOfBirth;
        }

        public Guid UserId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Description { get; }

        public Gender? Gender { get; }

        public string Country { get; }

        public string Telegram { get; }

        public string Skype { get; }

        public string Twitter { get; }

        public string Instagram { get; }

        public string Facebook { get; }

        public string Snapchat { get; }

        public string Youtube { get; }

        public string Blog { get; }

        public DateTime? DateOfBirth { get; }
    }
}
