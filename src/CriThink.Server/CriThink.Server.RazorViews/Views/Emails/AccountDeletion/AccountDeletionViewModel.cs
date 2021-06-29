namespace CriThink.Server.RazorViews.Views.Emails.AccountDeletion
{
    public class AccountDeletionViewModel
    {
        public AccountDeletionViewModel(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
