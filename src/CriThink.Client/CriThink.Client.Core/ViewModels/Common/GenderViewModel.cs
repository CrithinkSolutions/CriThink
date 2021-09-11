using CriThink.Common.Endpoints.DTOs.UserProfile;

namespace CriThink.Client.Core.ViewModels.Common
{
    public class GenderViewModel : BaseViewModel
    {
        public GenderViewModel(GenderDto? gender)
        {
            _gender = gender;
        }

        private GenderDto? _gender;

        public GenderDto? Gender
        {
            get => _gender;
            set
            {
                SetProperty(ref _gender, value);
                RaisePropertyChanged(nameof(LocalizedEntry));
            }
        }

        public string LocalizedEntry => _gender.HasValue ?
            LocalizedTextSource.GetText(_gender.Value.ToString()) :
            LocalizedTextSource.GetText("NoGender");
    }
}