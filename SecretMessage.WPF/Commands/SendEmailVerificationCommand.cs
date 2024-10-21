using MVVMEssentials.Commands;
using SecretMessage.WPF.Stores;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class SendEmailVerificationCommand : AsyncCommandBase
    {
        private readonly AuthenticationStore _authenticationStore;
        public SendEmailVerificationCommand(AuthenticationStore authenticationStore)
        {
            _authenticationStore = authenticationStore;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _authenticationStore.SendVerificationEmailAsync();

                MessageBox.Show("Verification email sent. Please check your email.", "Inform",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while sending email. Please check your email.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
