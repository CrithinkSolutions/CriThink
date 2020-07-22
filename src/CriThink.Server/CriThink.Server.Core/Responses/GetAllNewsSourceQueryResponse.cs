using System;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class GetAllNewsSourceQueryResponse
    {
        public GetAllNewsSourceQueryResponse(Uri uri, NewsSourceAuthencity sourceAuthencity)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            SourceAuthencity = sourceAuthencity;
        }

        public Uri Uri { get; }

        public NewsSourceAuthencity SourceAuthencity { get; }
    }
}
