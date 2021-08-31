using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class DeleteNotificationForUnknownSourceCommand : IRequest
    {
        public DeleteNotificationForUnknownSourceCommand(
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
