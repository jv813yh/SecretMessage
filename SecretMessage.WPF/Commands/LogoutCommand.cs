using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.Stores;

namespace SecretMessage.WPF.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly AuthenticationStore _authenticationStore;
        private readonly INavigationService _navigationServiceToLogout;

        public LogoutCommand(AuthenticationStore authenticationStore, INavigationService navigationServiceToLogout)
        {
            _authenticationStore = authenticationStore;
            _navigationServiceToLogout = navigationServiceToLogout;
        }
        public override void Execute(object parameter)
        {
            _authenticationStore.Logout();

            _navigationServiceToLogout.Navigate();
        }
    }
}
