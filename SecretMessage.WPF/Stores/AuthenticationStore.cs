using Firebase.Auth;

namespace SecretMessage.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private FirebaseAuthLink _firebaseAuthLink;

        public User? User 
            => _firebaseAuthLink?.User;


        public AuthenticationStore(FirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }

        public async Task LoginAsync(string email, string password)
        {
            _firebaseAuthLink = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
        }
    }
}
