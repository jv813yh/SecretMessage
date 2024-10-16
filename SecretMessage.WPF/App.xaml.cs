using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
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

                    services.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey)));

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<ModalNavigationStore>();

                    services.AddSingleton<NavigationService<RegisterViewModel>>(
                        (services) => new NavigationService<RegisterViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new RegisterViewModel(
                                services.GetRequiredService<FirebaseAuthProvider>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>(),
                                services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    services.AddSingleton<NavigationService<LoginViewModel>>(
                        (services) => new NavigationService<LoginViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new LoginViewModel(
                                services.GetRequiredService<FirebaseAuthProvider>(),
                                services.GetRequiredService<NavigationService<RegisterViewModel>>())));

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
