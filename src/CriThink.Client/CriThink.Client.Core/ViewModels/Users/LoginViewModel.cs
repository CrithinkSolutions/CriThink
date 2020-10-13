using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel()
        {

        }

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??= new MvxAsyncCommand(DoLoginCommand);

        private IMvxAsyncCommand _forgotPasswordCommand;
        public IMvxAsyncCommand ForgotPasswordCommand => _forgotPasswordCommand ??= new MvxAsyncCommand(DoForgotPasswordCommand);

        private Task DoForgotPasswordCommand()
        {
            return Task.CompletedTask;
        }

        private Task DoLoginCommand()
        {
            return Task.CompletedTask;
        }
    }
}
