using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.QueryResults
{
    public class UnknownNewsSourceQueryResult
    {
        public string Domain { get; set; }

        public NewsSourceAuthenticity NewsSourceAuthenticity { get; set; }
    }
}
