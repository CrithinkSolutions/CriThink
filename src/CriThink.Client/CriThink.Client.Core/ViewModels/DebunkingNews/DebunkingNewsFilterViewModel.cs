using System;

namespace CriThink.Client.Core.ViewModels.DebunkingNews
{
    public class DebunkingNewsFilterViewModel : BaseViewModel, IComparable<DebunkingNewsFilterViewModel>
    {
        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int CompareTo(DebunkingNewsFilterViewModel other)
        {
            if (other is null)
                return 1;

            return Code.CompareTo(other.Code);
        }
    }
}
