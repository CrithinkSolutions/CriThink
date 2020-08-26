using System;

namespace CriThink.Server.Core.Entities
{
    public class QuestionAnswer : ICriThinkIdentity
    {
        public Guid Id { get; set; }

        public bool IsTrue { get; set; }

        #region ForeignKey

        public Question Question { get; set; }

        public DemoNews DemoNews { get; set; }

        #endregion
    }
}
