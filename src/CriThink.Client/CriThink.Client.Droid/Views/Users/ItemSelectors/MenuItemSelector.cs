using CriThink.Client.Core.Models.Menu;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    public class MenuItemSelector : MvxTemplateSelector<BaseMenuItem>
    {
        private const int Header = 1;
        private const int Menu = 2;
        private const int Action = 3;
        private const int Version = 4;

        public MenuItemSelector()
        {
            ItemTemplateId = Resource.Layout.cell_header;
        }

        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType switch
            {
                Header => Resource.Layout.cell_header,
                Menu => Resource.Layout.cell_menu,
                Action => Resource.Layout.cell_action,
                Version => Resource.Layout.cell_version,
                _ => ItemTemplateId
            };
        }

        protected override int SelectItemViewType(BaseMenuItem forItemObject)
        {
            return forItemObject switch
            {
                HeaderModel _ => Header,
                MenuModel _ => Menu,
                ActionModel _ => Action,
                VersionModel _ => Version,
                _ => -1
            };
        }
    }
}