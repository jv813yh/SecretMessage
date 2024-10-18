using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Stores;

namespace SecretMessage.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly AuthenticationStore _authenticationStore;
        public string Username 
            => _authenticationStore.User?.DisplayName ?? "Unknown";

        public HomeViewModel(AuthenticationStore authenticationStore)
        {
            _authenticationStore = authenticationStore;
        }
    }
}
