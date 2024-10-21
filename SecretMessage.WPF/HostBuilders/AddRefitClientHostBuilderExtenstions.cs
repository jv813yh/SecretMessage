using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using SecretMessage.WPF.Http;
using SecretMessage.WPF.Queries;

namespace SecretMessage.WPF.HostBuilders
{
    public static class AddRefitClientHostBuilderExtenstions
    {
        public static IHostBuilder AddRefitClient(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // Get the base url for the SecretMessage API
                string? secretMessageApiBaseUrl = context.Configuration.GetValue<string>("SECRET_MESSAGE_API_BASE_URL");
                if (string.IsNullOrWhiteSpace(secretMessageApiBaseUrl))
                {
                    throw new Exception("Secret Message API Base URL is missing");
                }

                serviceCollection.AddTransient<FirebaseAuthHttpMessageHandler>();

                serviceCollection.AddRefitClient<IGetSecretMessageQuery>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(secretMessageApiBaseUrl))
                    .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();
            });

            return hostBuilder;
        }
    }
}
