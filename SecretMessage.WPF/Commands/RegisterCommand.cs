using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.ViewModels;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private readonly INavigationService _navigationService;

        public RegisterCommand(RegisterViewModel registerViewModel, 
                              FirebaseAuthProvider firebaseAuthProvider,
                              INavigationService navigationServiceToLogin)
        {
            _registerViewModel = registerViewModel;
            _firebaseAuthProvider = firebaseAuthProvider;
            _navigationService = navigationServiceToLogin;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            string password = _registerViewModel.Password;
            string confirmPassword = _registerViewModel.ConfirmPassword;

            if(password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            try
            {
                // Register the user with Firebase
                await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(
                    _registerViewModel.Email, 
                    _registerViewModel.Password, 
                    _registerViewModel.Username);

                MessageBox.Show("Successfully registered!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate to the login page
                _navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to register user.\nPlease check your register information or try again later.", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
