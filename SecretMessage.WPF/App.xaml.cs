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
        private string? _firebaseApiKey;

        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Get the Firebase API Key from the configuration file
                    _firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
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

                    services.AddTransient<FirebaseAuthHttpMessageHandler>();

                    // Register the FirebaseAuthProvider with the Firebase API Key
                    services.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey)));

                    // Register the Refit client for the SecretMessage API
                    services.AddRefitClient<IGetSecretMessageQuery>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(secretMessageApiBaseUrl))
                        .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<ModalNavigationStore>();

                    services.AddSingleton<AuthenticationStore>(
                        (services) => new AuthenticationStore(
                            services.GetRequiredService<FirebaseAuthProvider>()));

                    // Set DI for RegisterViewModel
                    services.AddSingleton<NavigationService<RegisterViewModel>>(
                        (services) => new NavigationService<RegisterViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new RegisterViewModel(
                                services.GetRequiredService<FirebaseAuthProvider>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    // Set DI for LoginViewModel
                    services.AddSingleton<NavigationService<LoginViewModel>>(
                        (services) => new NavigationService<LoginViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new LoginViewModel(
                                services.GetRequiredService<AuthenticationStore>(),
                                services.GetRequiredService<NavigationService<RegisterViewModel>>(),
                                services.GetRequiredService<NavigationService<HomeViewModel>>())));

                    // Set DI for HomeViewModel
                    services.AddSingleton<NavigationService<HomeViewModel>>(
                        (services) => new NavigationService<HomeViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => HomeViewModel.LoadHomeViewModel(
                                services.GetRequiredService<AuthenticationStore>(),
                                services.GetRequiredService<IGetSecretMessageQuery>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>())));



                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    }); 
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
