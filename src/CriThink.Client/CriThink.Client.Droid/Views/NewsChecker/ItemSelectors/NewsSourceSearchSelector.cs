using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    public class NewsSourceSearchSelector : MvxTemplateSelector<BaseNewsSourceSearch>
    {
        private const int DebunkingNews = 1;
        private const int Community = 2;

        public NewsSourceSearchSelector()
        {
            ItemTemplateId = Resource.Layout.cell_search_community;
        }

        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType switch
            {
                Community => Resource.Layout.cell_search_community,
                DebunkingNews => Resource.Layout.cell_search_debunkingnews,
                _ => ItemTemplateId
            };
        }

        protected override int SelectItemViewType(BaseNewsSourceSearch forItemObject)
        {
            if (forItemObject is NewsSourceSearchByTextResponse)
                return Community;

            return DebunkingNews;
        }
    }
}