using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SecretMessage.WPF.HostBuilders
{
    public static class AddFirebaseAuthProviderHostBuilderExtensions
    {
        public static IHostBuilder AddFirebaseAuthProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // Get the Firebase API Key from the configuration file
                string? _firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
                if (string.IsNullOrWhiteSpace(_firebaseApiKey))
                {
                    throw new Exception("Firebase API Key is missing");
                }

                // Register the FirebaseAuthProvider with the Firebase API Key
                serviceCollection.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey)));
            });

            return hostBuilder;
        }
    }
}
