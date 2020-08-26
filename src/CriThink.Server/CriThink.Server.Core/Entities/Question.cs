using System;

namespace CriThink.Server.Core.Entities
{
    public class Question : ICriThinkIdentity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
