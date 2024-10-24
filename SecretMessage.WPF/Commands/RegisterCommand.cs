﻿using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using SecretMessage.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace SecretMessage.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase, IDisposable
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

            _registerViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_registerViewModel.Email) 
              || e.PropertyName == nameof(_registerViewModel.Password)
              || e.PropertyName == nameof(_registerViewModel.ConfirmPassword)
              || e.PropertyName == nameof(_registerViewModel.Username))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
         => !string.IsNullOrEmpty(_registerViewModel.Email)
            && !string.IsNullOrEmpty(_registerViewModel.Password)
            && !string.IsNullOrEmpty(_registerViewModel.ConfirmPassword)
            && !string.IsNullOrEmpty(_registerViewModel.Username)
            && base.CanExecute(parameter);


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
                    _registerViewModel.Username,
                    true);

                MessageBox.Show("Successfully registered!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);

                Dispose();

                // Navigate to the login page
                _navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to register user.\nPlease check your register information or try again later.", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Dispose()
        {
            _registerViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }
}
