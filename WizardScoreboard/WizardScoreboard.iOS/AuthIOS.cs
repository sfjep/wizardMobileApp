using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using WizardScoreboard;
using WizardScoreboard.iOS;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;

[assembly: Dependency(typeof(AuthIOS))]
namespace WizardScoreboard.iOS
{
    public class AuthIOS : IAuth
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return ""; 
            }

        }
    }
}