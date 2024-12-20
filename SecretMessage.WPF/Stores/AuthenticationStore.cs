﻿using Firebase.Auth;
using System.Text.Json;

namespace SecretMessage.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private FirebaseAuthLink _currentFirebaseAuthLink;

        public User? User 
            => _currentFirebaseAuthLink?.User;

        public bool IsLoggedIn
            => (!_currentFirebaseAuthLink?.IsExpired()) ?? false;


        public AuthenticationStore(FirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }

        public async Task LoginAsync(string email, string password)
        {
            // Sign in with the email and password
            // Get the Firebase token
            _currentFirebaseAuthLink = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);

            // Save the Firebase token to the user's settings
            SaveAuthenticationState(_currentFirebaseAuthLink);
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
                    _currentFirebaseAuthLink = new FirebaseAuthLink(_firebaseAuthProvider, firebaseAuth);

                     await UpdateFreshAuthAsync();
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
            if (_currentFirebaseAuthLink == null)
            {
                return;
            }

            _currentFirebaseAuthLink = await _currentFirebaseAuthLink.GetFreshAuthAsync();

            // Save the Firebase token to the user's settings
            SaveAuthenticationState(_currentFirebaseAuthLink);

        }

        /// <summary>
        /// Delete the user's account
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAccount()
        {
            if (_currentFirebaseAuthLink == null)
            {
                return;
            }

            await _firebaseAuthProvider.DeleteUserAsync(_currentFirebaseAuthLink.FirebaseToken);

            // Clear the Firebase token from the user's settings
            ClearAuthenticationState();
        }

        /// <summary>
        /// Send verification email to the user
        /// </summary>
        /// <returns></returns>
        public async Task SendVerificationEmailAsync()
        {
            if (_currentFirebaseAuthLink == null)
            {
                throw new Exception("User is not authenticated");
            }

            // Update fresh auth token
            await UpdateFreshAuthAsync();

            // Send the verification email
            await _firebaseAuthProvider.SendEmailVerificationAsync(_currentFirebaseAuthLink.FirebaseToken);
        }

        public string? GetFirebaseToken()
        {
            return _currentFirebaseAuthLink?.FirebaseToken;
        }

        public void Logout()
        {
            _currentFirebaseAuthLink = null;

            // Clear the Firebase token from the user's settings
            ClearAuthenticationState();
        }

        // Save the Firebase token to the user's settings
        private void SaveAuthenticationState(FirebaseAuthLink firebaseAuthLink)
        {
            string fireBaseAuthLinkJson = JsonSerializer.Serialize(firebaseAuthLink);

            // Set the JSON to the user's settings,
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
