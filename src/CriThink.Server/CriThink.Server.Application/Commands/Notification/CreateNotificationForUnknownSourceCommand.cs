using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateNotificationForUnknownSourceCommand : IRequest
    {
        public CreateNotificationForUnknownSourceCommand(
            string newsSource,
            string userEmail)
        {
            NewsSource = newsSource;
            UserEmail = userEmail;
        }

        public string NewsSource { get; }

        public string UserEmail { get; }
    }
}
