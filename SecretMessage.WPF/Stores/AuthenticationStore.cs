using Firebase.Auth;
using System.Text.Json;

namespace SecretMessage.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private FirebaseAuthLink _firebaseAuthLink;

        public User? User 
            => _firebaseAuthLink?.User;

        public bool IsLoggedIn
            => (!_firebaseAuthLink?.IsExpired()) ?? false;


        public AuthenticationStore(FirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }

        public async Task LoginAsync(string email, string password)
        {
            // Sign in with the email and password
            // Get the Firebase token
            _firebaseAuthLink = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);

            // Save the Firebase token to the user's settings
            SaveAuthenticationState(_firebaseAuthLink);
        }

        public async Task InitializeAsync()
        {
            // Get the Firebase token from the user's settings
            string firebaseAuthLinkJson = Properties.Settings.Default.FirebaseAuthLink;
            if (!string.IsNullOrEmpty(firebaseAuthLinkJson))
            {
                FirebaseAuth firebaseAuth = JsonSerializer.Deserialize<FirebaseAuth>(firebaseAuthLinkJson);

                if (firebaseAuth != null)
                {
                     await UpdateFreshAuthAsync();

                    _firebaseAuthLink = new FirebaseAuthLink(_firebaseAuthProvider, firebaseAuth);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }


        // Get a fresh auth token
        public async Task UpdateFreshAuthAsync()
        {
            if (_firebaseAuthLink == null)
            {
                return;
            }

            _firebaseAuthLink = await _firebaseAuthLink.GetFreshAuthAsync();

            // Save the Firebase token to the user's settings
            SaveAuthenticationState(_firebaseAuthLink);

        }

        public string? GetFirebaseToken()
        {
            return _firebaseAuthLink?.FirebaseToken;
        }

        public void Logout()
        {
            _firebaseAuthLink = null;

            // Clear the Firebase token from the user's settings
            ClearAuthenticationState();
        }

        // Save the Firebase token to the user's settings
        private void SaveAuthenticationState(FirebaseAuthLink firebaseAuthLink)
        {
            string fireBaseAuthLinkJson = JsonSerializer.Serialize(firebaseAuthLink);

            // Set the JSON to the user's settings,
            // set the expiration time and save the settings
            Properties.Settings.Default.FirebaseAuthLink = fireBaseAuthLinkJson;
            Properties.Settings.Default.Save();
        }

        // Clear the Firebase token from the user's settings
        private void ClearAuthenticationState()
        {
            Properties.Settings.Default.FirebaseAuthLink = null;
            Properties.Settings.Default.Save();
        }
    }
}
