using System;

namespace CriThink.Server.Core.Entities
{
    public class DemoNews : ICriThinkIdentity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }
    }
}
