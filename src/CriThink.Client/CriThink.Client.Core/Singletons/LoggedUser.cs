using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Singletons
{
    internal class LoggedUser
    {
        private LoggedUser() { }

        public static User Instance { get; private set; }

        public static void Login(UserLoginResponse userLoginResponse)
        {
            Instance = new User(userLoginResponse.UserId, userLoginResponse.UserEmail, userLoginResponse.UserName, userLoginResponse.JwtToken);
        }

        public static void Login(VerifyUserEmailResponse userData)
        {
            Instance = new User(userData.UserId, userData.UserEmail, userData.Username, userData.JwtToken);
        }

        public static void Logout()
        {
            Instance = null;
        }
    }
}
