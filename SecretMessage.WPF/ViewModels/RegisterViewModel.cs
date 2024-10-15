using Firebase.Auth;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Commands;
using System.Windows.Input;

namespace SecretMessage.WPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get => _username;
            set 
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand NavigateLoginCommand { get; }


        public RegisterViewModel(FirebaseAuthProvider firebaseAuthProvider)
        {
            SubmitCommand = new RegisterCommand(this, firebaseAuthProvider);
        }

    }
}
