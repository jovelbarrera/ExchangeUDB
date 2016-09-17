using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Dependencies.Facebook;
using Exchange.Helpers;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Services.AuthService;
using Firebase.Xamarin.Auth;

namespace Exchange.Services.FirebaseServices
{
    public class FirebaseAuthProvider : AuthProvider<FirebaseAuthProvider, PersistentUser>
    {
        private Firebase.Xamarin.Auth.FirebaseAuthProvider authProvider;

        private IUserManager<PersistentUser> UserManager
        {
            get
            {
                return CustomUserManager.Instance;
            }
        }

        public FirebaseAuthProvider()
        {
            var config = new FirebaseConfig(FirebaseAccess.Instance.ApiKey);
            authProvider = new Firebase.Xamarin.Auth.FirebaseAuthProvider(config);
        }

        public override async Task LogIn(string email, string password)
        {
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            await RegisteredUser(auth);
        }

        public override async Task LogIn(FacebookToken facebookToken)
        {
            FirebaseAuthLink auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, facebookToken.AccessToken);
            await RegisteredUser(auth);
        }

        public override async Task SignUp(string email, string password)
        {
            FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            await RegisteredUser(auth);
        }

        public override async Task ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
        }

        public async Task<PersistentUser> RegisteredUser(FirebaseAuthLink auth)
        {
            if (auth == null || auth.User == null)
                throw new Exception("Unexpected Error");

            var user = new PersistentUser();
            user.DisplayName = auth.User.DisplayName;
            user.Email = auth.User.Email;
            user.ObjectId = auth.User.LocalId;
            user.FirstName = auth.User.FirstName;
            user.LastName = auth.User.LastName;
            user.ProfilePicture = auth.User.PhotoUrl;

            Settings.FirebaseUserToken = auth.FirebaseToken;
            Settings.FirebaseUserRefreshToken = auth.RefreshToken;
            Settings.FirebaseUserTokenExpiration = (await TimeService.Instance.Now()).AddSeconds(auth.ExpiresIn - 60);

            await UserManager.SetCurrentUser(user);

            return user;
        }
    }
}

