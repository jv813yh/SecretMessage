using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Commands;
using System.Windows.Input;

namespace SecretMessage.WPF.ViewModels
{
    public class PasswordResetViewModel : ViewModelBase
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

        public ICommand ResetPasswordCommand { get; }
        public ICommand NavigateLoginCommand { get; }

        public PasswordResetViewModel(FirebaseAuthProvider firebaseAuthProvider, 
                                      NavigationService<LoginViewModel> navigationServiceToLoginViewModel)
        {
            ResetPasswordCommand = new SendPasswordResetEmailCommand(firebaseAuthProvider, this, navigationServiceToLoginViewModel);
            NavigateLoginCommand = new NavigateCommand(navigationServiceToLoginViewModel);
        }
    }
}
