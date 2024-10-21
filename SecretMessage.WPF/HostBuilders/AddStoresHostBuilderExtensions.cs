using Firebase.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Stores;
using SecretMessage.WPF.Stores;

namespace SecretMessage.WPF.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {
                // Register states 
                serviceCollection.AddSingleton<NavigationStore>();
                serviceCollection.AddSingleton<ModalNavigationStore>();

                serviceCollection.AddSingleton<AuthenticationStore>(
               (services) => new AuthenticationStore(
                   services.GetRequiredService<FirebaseAuthProvider>()));

            });

            return hostBuilder;
        }
    }
}
