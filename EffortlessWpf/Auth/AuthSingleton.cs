using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.Auth
{
    public sealed class AuthSingleton
    {
        private static readonly Lazy<AuthSingleton> _lazyInstance = new Lazy<AuthSingleton>(() => new AuthSingleton());
        public TokenModel AuthToken { get; private set; }

        public event EventHandler<TokenModel> OnLoginEvent;
        public event EventHandler<TokenModel> OnLogoutEvent;

        private AuthSingleton() { }

        public static AuthSingleton Instance => _lazyInstance.Value;

        public bool IsAuthenticated()
        {
            return AuthToken != null;
        }

        public void Login(TokenModel token)
        {
            if (token == null) return;

            AuthToken = token;
            OnLoginEvent?.Invoke(this, token);
        }

        public void Logout()
        {
            AuthToken = null;
            OnLogoutEvent?.Invoke(this, null);
        }
    }
}
