using MVVMEssentials.Commands;
using SecretMessage.Core;
using SecretMessage.WPF.Queries;
using SecretMessage.WPF.ViewModels;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class LoadSecretMessageCommand : AsyncCommandBase
    {
        private readonly HomeViewModel _homeViewModel;
        // Refit query to get the secret message
        private readonly IGetSecretMessageQuery _getSecretMessageQuery;

        public LoadSecretMessageCommand(HomeViewModel homeViewModel, IGetSecretMessageQuery getSecretMessageQuery)
        {
            _homeViewModel = homeViewModel;
            _getSecretMessageQuery = getSecretMessageQuery;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Get the secret message from th
                SecretMessageResponse secretMessageResponse = await _getSecretMessageQuery.ExecuteAsync();

                // Set the secret message in the view model
                _homeViewModel.SecretMessage = secretMessageResponse.SecretMessage;

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load the secret message.\nPlease try again later.", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
