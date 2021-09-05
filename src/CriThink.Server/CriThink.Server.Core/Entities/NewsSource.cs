using System;

namespace CriThink.Server.Core.Entities
{
    public class NewsSource
    {
        public NewsSource(
            string name,
            NewsSourceAuthenticity category)
        {
            Name = name;
            Category = category;
        }

        public NewsSource(
            string name,
            string category)
        {
            Name = name;
            Category = (NewsSourceAuthenticity) Enum.Parse(typeof(NewsSourceAuthenticity), category, true);
        }

        public string Name { get; }

        public NewsSourceAuthenticity Category { get; }

        public void UpdateAuthenticity(NewsSourceAuthenticity authenticity)
        {
            if (authenticity is NewsSourceAuthenticity.Suspicious &&
                (Category is NewsSourceAuthenticity.Conspiracist ||
                Category is NewsSourceAuthenticity.FakeNews))
            {
                throw new InvalidOperationException($"You can't convert a news source from {nameof(NewsSourceAuthenticity.Suspicious)} to {authenticity}");
            }
        }
    }
}
