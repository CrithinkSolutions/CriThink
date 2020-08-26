using System;

namespace CriThink.Server.Core.Responses
{
    public class GetAllDemoNewsQueryResponse
    {
        public GetAllDemoNewsQueryResponse(Guid id, string title, string link)
        {
            Id = id;
            Title = title;

            if (!string.IsNullOrWhiteSpace(link))
                Uri = new Uri(link);
        }

        public Guid Id { get; }

        public string Title { get; }

        public Uri Uri { get; }
    }
}
