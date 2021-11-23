using CriThink.Client.Core.Models.NewsChecker;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    public class RecentSearchSelector : MvxTemplateSelector<RecentNewsChecksModel>
    {
        private const int NewsCheck = 1;
        private const int Search = 2;

        public RecentSearchSelector()
        {
            ItemTemplateId = Resource.Layout.cell_recentsearches_feed;
        }

        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType switch
            {
                NewsCheck => Resource.Layout.cell_recentnewscheck_feed,
                Search => Resource.Layout.cell_recentsearches_feed,
                _ => ItemTemplateId
            };
        }

        protected override int SelectItemViewType(RecentNewsChecksModel forItemObject)
        {
            return string.IsNullOrWhiteSpace(forItemObject.Title) ?
                Search :
                NewsCheck;
        }
    }
}