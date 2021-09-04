using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CriThink.Server.Core.DomainServices;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNews : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserver constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNews()
        { }

        private DebunkingNews(
            string title,
            string link,
            string imageLink,
            DateTime publishingDate)
        {
            Title = title;
            Link = link;
            ImageLink = imageLink;
            PublishingDate = publishingDate;
        }

        private DebunkingNews(
            string title,
            string link,
            DateTime publishingDate)
        {
            Title = title;
            Link = link;
            PublishingDate = publishingDate;
        }

        private DebunkingNews(
            string title,
            string newsCaption,
            string link,
            string imageLink,
            string keywords)
        {
            Title = title;
            NewsCaption = newsCaption;
            Link = link;
            ImageLink = imageLink;
            Keywords = keywords;
        }

        public string Title { get; private set; }

        public DateTimeOffset? PublishingDate { get; private set; }

        public string Link { get; private set; }

        public string ImageLink { get; private set; }

        public string NewsCaption { get; private set; }

        public string Keywords { get; private set; }

        #region Relationships

        public virtual Guid PublisherId { get; private set; }

        public virtual DebunkingNewsPublisher Publisher { get; private set; }

        #endregion

        #region Create

        public static DebunkingNews Create(
            string title,
            string link,
            DateTimeOffset publishingDate)
        {
            return new DebunkingNews(
                title,
                link,
                publishingDate.DateTime);
        }

        public static DebunkingNews Create(
            string title,
            string link,
            string imageLink,
            DateTimeOffset publishingDate)
        {
            return new DebunkingNews(
                title,
                link,
                imageLink,
                publishingDate.DateTime);
        }

        public static DebunkingNews Create(
            string title,
            string newsCaption,
            string link,
            string imageLink,
            IEnumerable<string> keywords)
        {
            return new DebunkingNews(
                title,
                newsCaption,
                link,
                imageLink,
                MergeKeywords(keywords));
        }

        #endregion

        public void SetPublisher(DebunkingNewsPublisher publisher)
        {
            Publisher = publisher;
        }

        public async Task SetPublisherAsync(
            IDebunkingNewsPublisherService publisherService,
            string publisherName)
        {
            if (publisherService is null)
                throw new ArgumentNullException(nameof(publisherService));

            var publisher = await publisherService.GetDebunkingNewsPublisherByNameAsync(publisherName);
            Publisher = publisher;
        }

        public async Task SetPublisherAsync(
            IDebunkingNewsPublisherService publisherService,
            Guid publisherId)
        {
            if (publisherService is null)
                throw new ArgumentNullException(nameof(publisherService));

            var publisher = await publisherService.GetDebunkingNewsPublisherByIdAsync(publisherId);
            Publisher = publisher;
        }

        public void UpdateNewsCaption(string caption)
        {
            NewsCaption = caption;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateLink(string link)
        {
            Link = link;
        }

        public void UpdateImageLink(string imageLink)
        {
            ImageLink = imageLink;
        }

        public void SetKeywords(IEnumerable<string> keywords)
        {
            Keywords = MergeKeywords(keywords);
        }

        private static string MergeKeywords(IEnumerable<string> keywords) =>
            string.Join(',', keywords);
    }
}
