using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Commands;
using SecretMessage.WPF.Stores;
using System.Windows.Input;

namespace SecretMessage.WPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly AuthenticationStore _authenticationStore;

        public string Email 
            => _authenticationStore.User?.Email ?? string.Empty;
        public string Username
            => _authenticationStore.User?.DisplayName ?? string.Empty;

        public bool IsEmailVerified 
            => _authenticationStore.User?.IsEmailVerified ?? false;

        public ICommand NavigateHomeCommand { get; }
        public ICommand SendEmailVerificationCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        public ProfileViewModel(AuthenticationStore authenticationStore, 
                                INavigationService navigateToHomeViewModel,
                                INavigationService navigateToLoginViewModel)
        {
            _authenticationStore = authenticationStore;
            NavigateHomeCommand = new NavigateCommand(navigateToHomeViewModel);
            SendEmailVerificationCommand = new SendEmailVerificationCommand(authenticationStore);
            DeleteAccountCommand = new DeleteAccountCommand(authenticationStore, navigateToLoginViewModel); 
        }
    }
}
