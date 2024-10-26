using Firebase.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Refit;
using SecretMessage.WPF.HostBuilders;
using SecretMessage.WPF.Stores;
using SecretMessage.WPF.ViewModels;
using System.Windows;

namespace SecretMessage.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .AddFirebaseAuthProvider()
                .AddRefitClient()
                .AddStores()
                .AddViewModels()
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    }); 
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await InitializeAppAsync();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }

        private async Task InitializeAppAsync()
        {
            try
            {
                // Get the AuthenticationStore from the DI container
                AuthenticationStore authStore = _host.Services.GetRequiredService<AuthenticationStore>();
                // Initialize the AuthenticationStore
                await authStore.InitializeAsync();

                if (authStore.IsLoggedIn)
                {
                    INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<HomeViewModel>>();
                    navigationService.Navigate();
                }
                else
                {
                    INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
                    navigationService.Navigate();
                }
            }
            catch (FirebaseAuthException)
            {
                INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
                navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load application", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
