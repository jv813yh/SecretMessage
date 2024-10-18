using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.Stores;
using SecretMessage.WPF.ViewModels;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly AuthenticationStore _authenticationStore;
        private readonly INavigationService _navigationService;
        public LoginCommand(LoginViewModel loginViewModel,
                            AuthenticationStore authenticationStore,
                            INavigationService navigationServiceToHome)
        {
            _loginViewModel = loginViewModel;
            _authenticationStore = authenticationStore;
            _navigationService = navigationServiceToHome;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Sign in with the email and password
                await _authenticationStore.LoginAsync(_loginViewModel.Email, _loginViewModel.Password);

                MessageBox.Show("Successfully logged in!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate to the main page
                _navigationService.Navigate();  
            }
            catch (Exception)
            {

                MessageBox.Show("Invalid email or password. Please check your information or try again later.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
