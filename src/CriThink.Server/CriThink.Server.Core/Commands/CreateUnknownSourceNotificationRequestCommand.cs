using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateUnknownSourceNotificationRequestCommand : IRequest
    {
        public CreateUnknownSourceNotificationRequestCommand(string domain, string email)
        {
            Domain = domain;
            Email = email;
        }

        public string Domain { get; set; }

        public string Email { get; set; }
    }
}
