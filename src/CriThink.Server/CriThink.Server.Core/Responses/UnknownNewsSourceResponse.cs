using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class UnknownNewsSourceResponse
    {
        public string Domain { get; set; }

        public NewsSourceAuthenticity NewsSourceAuthenticity { get; set; }
    }
}
