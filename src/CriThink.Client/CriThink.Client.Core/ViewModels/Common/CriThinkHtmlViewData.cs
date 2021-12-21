using System;

namespace CriThink.Client.Core.ViewModels.Common
{
    public class CriThinkHtmlViewData
    {
        public CriThinkHtmlViewData(
            string title,
            Uri uri)
        {
            Title = title;
            Uri = uri;
        }

        public Uri Uri { get; }

        public string Title { get; }
    }
}
