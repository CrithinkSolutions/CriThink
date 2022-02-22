using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class AddUserSearchCommand : IRequest
    {
        public AddUserSearchCommand(
            Guid userId,
            string searchedText)
        {
            UserId = userId;
            SearchedText = searchedText;
        }

        public Guid UserId { get; }

        public string SearchedText { get; }
    }
}
