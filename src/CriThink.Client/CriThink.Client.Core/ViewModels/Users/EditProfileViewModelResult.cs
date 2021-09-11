namespace CriThink.Client.Core.ViewModels.Users
{
    public class EditProfileViewModelResult
    {
        public EditProfileViewModelResult(bool hasBeenEdited)
        {
            HasBeenEdited = hasBeenEdited;
        }

        public bool HasBeenEdited { get; }
    }
}
