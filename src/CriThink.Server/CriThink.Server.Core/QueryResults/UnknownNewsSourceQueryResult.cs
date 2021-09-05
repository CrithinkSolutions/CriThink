using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.QueryResults
{
    public class UnknownNewsSourceQueryResult
    {
        public string Domain { get; set; }

        public NewsSourceAuthenticity NewsSourceAuthenticity { get; set; }
    }
}
