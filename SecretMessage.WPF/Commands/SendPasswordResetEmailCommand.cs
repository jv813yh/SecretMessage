using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class SendPasswordResetEmailCommand : AsyncCommandBase, IDisposable
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private readonly PasswordResetViewModel _passwordResetViewModel;
        private readonly INavigationService _navigationServiceToLoginViewModel;

        public SendPasswordResetEmailCommand(FirebaseAuthProvider firebaseAuthProvider, 
                                             PasswordResetViewModel passwordResetViewModel,
                                             INavigationService navigationServiceToLoginViewModel)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
            _passwordResetViewModel = passwordResetViewModel;
            _navigationServiceToLoginViewModel = navigationServiceToLoginViewModel;

            _passwordResetViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_passwordResetViewModel.Email))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
         => !string.IsNullOrEmpty(_passwordResetViewModel.Email) 
                    && base.CanExecute(parameter);

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Send the password reset email
                await _firebaseAuthProvider.SendPasswordResetEmailAsync(_passwordResetViewModel.Email);

                MessageBox.Show("Password reset email sent successfully!\nCheck your email to finish resetting your password", 
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Dispose the resources
                Dispose();

                // Navigate to the login page
                _navigationServiceToLoginViewModel.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to send the password reset email.\nPlease try again later.", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Dispose()
        {
            _passwordResetViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }
}
