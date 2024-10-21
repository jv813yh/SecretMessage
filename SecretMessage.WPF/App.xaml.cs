using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using Refit;
using SecretMessage.WPF.Http;
using SecretMessage.WPF.Queries;
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
                .ConfigureServices((context, serviceCollection) =>
                {
                // Get the Firebase API Key from the configuration file
                string? _firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
                if (string.IsNullOrWhiteSpace(_firebaseApiKey))
                {
                    throw new Exception("Firebase API Key is missing");
                }

                // Get the base url for the SecretMessage API
                string? secretMessageApiBaseUrl = context.Configuration.GetValue<string>("SECRET_MESSAGE_API_BASE_URL");
                if (string.IsNullOrWhiteSpace(secretMessageApiBaseUrl))
                {
                    throw new Exception("Secret Message API Base URL is missing");
                }

                serviceCollection.AddTransient<FirebaseAuthHttpMessageHandler>();

                // Register the FirebaseAuthProvider with the Firebase API Key
                serviceCollection.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey)));

                // Register the Refit client for the SecretMessage API
                serviceCollection.AddRefitClient<IGetSecretMessageQuery>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(secretMessageApiBaseUrl))
                    .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();

                // Register states 
                serviceCollection.AddSingleton<NavigationStore>();
                serviceCollection.AddSingleton<ModalNavigationStore>();

                serviceCollection.AddSingleton<AuthenticationStore>(
                    (services) => new AuthenticationStore(
                        services.GetRequiredService<FirebaseAuthProvider>()));

                // Set DI for RegisterViewModel
                serviceCollection.AddSingleton<NavigationService<RegisterViewModel>>(
                    (services) => new NavigationService<RegisterViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new RegisterViewModel(
                            services.GetRequiredService<FirebaseAuthProvider>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                // Set DI for LoginViewModel
                serviceCollection.AddSingleton<NavigationService<LoginViewModel>>(
                    (services) => new NavigationService<LoginViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new LoginViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<NavigationService<RegisterViewModel>>(),
                            services.GetRequiredService<NavigationService<HomeViewModel>>(),
                            services.GetRequiredService<NavigationService<PasswordResetViewModel>>())));

                // Set DI for HomeViewModel
                serviceCollection.AddSingleton<NavigationService<HomeViewModel>>(
                    (services) => new NavigationService<HomeViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => HomeViewModel.LoadHomeViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<IGetSecretMessageQuery>(),
                            services.GetRequiredService<NavigationService<ProfileViewModel>>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                // Set DI for PasswordResetViewModel    
                serviceCollection.AddSingleton<NavigationService<PasswordResetViewModel>>(
                    (services) => new NavigationService<PasswordResetViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new PasswordResetViewModel(
                            services.GetRequiredService<FirebaseAuthProvider>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                // Set DI for ProfileViewModel
                serviceCollection.AddSingleton<NavigationService<ProfileViewModel>>(
                    (services) => new NavigationService<ProfileViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new ProfileViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<NavigationService<HomeViewModel>>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));


                    serviceCollection.AddSingleton<MainViewModel>();
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
