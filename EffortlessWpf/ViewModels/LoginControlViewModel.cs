using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EffortlessWpf.ViewModels
{
    public class LoginControlViewModel : Screen
    {
        public string UserName { get; set; }
        public SecureString SecurePassword { get; set; }
        public string Password { private get; set; }
        public UserModel User => new UserModel { UserName = UserName, Password = Password };
        public bool IsLoginFormEnabled { get; private set; } = true;

        public event EventHandler<TokenModel> OnSuccessfullLogin;

        private AuthAccess _authAccess;
        private string _apiUrl;

        public string ApiUrl
        {
            get { return _apiUrl; }
            set
            {
                _apiUrl = value;
                _authAccess = new AuthAccess(ApiUrl);
                UserName = "";
                Password = "";
            }
        }


        public LoginControlViewModel(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public async Task Login()
        {
            IsLoginFormEnabled = false;
            NotifyOfPropertyChange(() => IsLoginFormEnabled);
            Debug.WriteLine($"Attempting login with username {UserName} and {Password}");

            TokenModel loginToken = null;

            try
            {
                //loginToken = await _authAccess.Authenticate(User);
                loginToken = await _authAccess.Authenticate(UserName, Password);
            } 
            catch(Flurl.Http.FlurlHttpException ex)
            {
                var msg = ex.GetResponseJsonAsync().ToString();
                Debug.WriteLine($"I just failed so much, you wouldn't believe it: {ex.Message}, {msg}");
                MessageBox.Show($"Could not login. Either the server is down ({ApiUrl}) or the entered credentials were incorrect.\n\n{ex.Message}.", "Failed to login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoginFormEnabled = true;
                NotifyOfPropertyChange(() => IsLoginFormEnabled);
            }

            //var loginToken = await _authAccess.Authenticate(User);

            if (loginToken == null)
            {
                Debug.WriteLine("Failed to sign in.");
            }
            else
            {
                Debug.WriteLine($"Successfully signed in as {loginToken.User.FullNameCapitalized}.");

                AuthSingleton.Instance.Login(loginToken);

                OnSuccessfullLogin?.Invoke(this, loginToken);
            }
        }

        public void AutoLogin()
        {
            UserName = "TestBooker";
            Password = "SecurePassword";

            NotifyOfPropertyChange(() => UserName);

            Task.Run(async () => await Login());
        }

        public void SetPassword(PasswordBox box)
        {
            Password = box.Password;
        }
    }
}
