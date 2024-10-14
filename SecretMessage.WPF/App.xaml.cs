using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddSingleton(new MainWindow());
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
