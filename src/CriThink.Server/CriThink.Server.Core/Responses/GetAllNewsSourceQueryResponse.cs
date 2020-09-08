using System;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Responses
{
    public class GetAllNewsSourceQueryResponse
    {
        public GetAllNewsSourceQueryResponse(Uri uri, NewsSourceAuthenticity sourceAuthencity)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            SourceAuthencity = sourceAuthencity;
        }

        public Uri Uri { get; }

        public NewsSourceAuthenticity SourceAuthencity { get; }
    }
}
