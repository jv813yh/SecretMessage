using Firebase.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using SecretMessage.WPF.Queries;
using SecretMessage.WPF.Stores;
using SecretMessage.WPF.ViewModels;

namespace SecretMessage.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {

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

            });

            return hostBuilder;
        }
    }
}
