using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.Stores;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class DeleteAccountCommand : AsyncCommandBase
    {
        private readonly AuthenticationStore _authenticationStore;
        private readonly INavigationService _navigateToLoginViewModel;

        public DeleteAccountCommand(AuthenticationStore authenticationStore, 
                                    INavigationService navigateToLoginViewModel)
        {
            _authenticationStore = authenticationStore;
            _navigateToLoginViewModel = navigateToLoginViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Delete the account
                await _authenticationStore.DeleteAccount();

                MessageBox.Show("Account deleted successfully!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate to the login page after deleting the account
                _navigateToLoginViewModel.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete the account.\nPlease try again later.", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
