using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateTriggerLogCommand : IRequest
    {
        public CreateTriggerLogCommand()
        {
            IsSuccessful = true;
        }

        public CreateTriggerLogCommand(string failReason)
        {
            IsSuccessful = false;
            FailReason = failReason;
        }

        public bool IsSuccessful { get; }

        public string FailReason { get; }
    }
}
