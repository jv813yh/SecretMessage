using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Commands;
using SecretMessage.WPF.Queries;
using SecretMessage.WPF.Stores;
using System.Windows.Input;

namespace SecretMessage.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly AuthenticationStore _authenticationStore;
        public string Username 
            => _authenticationStore.User?.DisplayName ?? "Unknown";

        private string _secretMessage;
        public string SecretMessage 
        {
            get => _secretMessage;
            set
            {
                _secretMessage = value;
                OnPropertyChanged(nameof(SecretMessage));
            }
        }

        public ICommand LoadSecretMessageCommand { get; }
        public HomeViewModel(AuthenticationStore authenticationStore, IGetSecretMessageQuery getSecretMessageQuery)
        {
            _authenticationStore = authenticationStore;
            LoadSecretMessageCommand = new LoadSecretMessageCommand(this, getSecretMessageQuery);
        }

        public static HomeViewModel LoadHomeViewModel(AuthenticationStore authenticationStore, IGetSecretMessageQuery getSecretMessageQuery)
        {
            HomeViewModel returnHomeViewModel = new HomeViewModel(authenticationStore, getSecretMessageQuery);
            returnHomeViewModel.LoadSecretMessageCommand.Execute(null);

            return returnHomeViewModel;
        }
    }
}
