﻿using MVVMEssentials.Commands;
using MVVMEssentials.Services;
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
        public ICommand NavigateToProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeViewModel(AuthenticationStore authenticationStore, 
                             IGetSecretMessageQuery getSecretMessageQuery,
                             INavigationService navigationServiceToProfile,
                             INavigationService navigationServiceToLogout)
        {
            _authenticationStore = authenticationStore;
            LoadSecretMessageCommand = new LoadSecretMessageCommand(this, getSecretMessageQuery);
            NavigateToProfileCommand = new NavigateCommand(navigationServiceToProfile);
            LogoutCommand = new LogoutCommand(authenticationStore, navigationServiceToLogout);
        }

        public static HomeViewModel LoadHomeViewModel(AuthenticationStore authenticationStore, 
                                                      IGetSecretMessageQuery getSecretMessageQuery,
                                                      INavigationService navigationServiceToProfile,
                                                      INavigationService navigationServiceToLogout)
        {
            HomeViewModel returnHomeViewModel = new HomeViewModel(authenticationStore, getSecretMessageQuery, 
                navigationServiceToProfile, navigationServiceToLogout);

            returnHomeViewModel.LoadSecretMessageCommand.Execute(null);

            return returnHomeViewModel;
        }
    }
}
