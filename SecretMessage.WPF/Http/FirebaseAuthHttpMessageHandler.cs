using Firebase.Auth;
using SecretMessage.WPF.Stores;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SecretMessage.WPF.Http
{
    public class FirebaseAuthHttpMessageHandler : DelegatingHandler
    {
        private readonly AuthenticationStore _authenticationStore;

        public FirebaseAuthHttpMessageHandler(AuthenticationStore authenticationStore)
        {
            _authenticationStore = authenticationStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the Firebase token from the AuthenticationStore
            FirebaseAuthLink firebaseAuthLink = await _authenticationStore.GetFreshAuthAsync();
            // Add the Firebase token to the request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", firebaseAuthLink.FirebaseToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
