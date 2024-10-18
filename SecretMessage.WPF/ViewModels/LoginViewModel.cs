using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Commands;
using SecretMessage.WPF.Stores;
using System.Windows.Input;

namespace SecretMessage.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand NavigateRegisterCommand { get; }

        public LoginViewModel(AuthenticationStore authenticationStore, 
                              NavigationService<RegisterViewModel> registerViewModelNavigation,
                              NavigationService<HomeViewModel> homeViewModelNavigation)
        {
            LoginCommand = new LoginCommand(this, authenticationStore, homeViewModelNavigation);
            NavigateRegisterCommand = new NavigateCommand(registerViewModelNavigation);
        }
    }
}
