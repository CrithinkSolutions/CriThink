using System.Threading.Tasks;

namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsCheckerResultDetailViewModel : BaseViewModel
    {
        public NewsCheckerResultDetailViewModel()
        {

        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _classification;

        public string Classification
        {
            get => _classification;
            set => SetProperty(ref _classification, value);
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override async Task Initialize()
        {
            await base.Initialize();
        }
    }
}
