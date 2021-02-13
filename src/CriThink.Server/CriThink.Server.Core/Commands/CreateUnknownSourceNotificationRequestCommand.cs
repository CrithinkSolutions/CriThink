using MediatR;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Core.Commands
{
    public class CreateUnknownSourceNotificationRequestCommand : IRequest
    {
        public CreateUnknownSourceNotificationRequestCommand(string url, string email)
        {
            Url = url;
            Email = email;
        }

        public string Url { get; set; }

        public string Email { get; set; }
    }
}
