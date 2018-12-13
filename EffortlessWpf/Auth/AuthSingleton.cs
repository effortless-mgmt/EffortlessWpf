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
        public TokenModel AuthToken { get; set; }

        private AuthSingleton() { }

        public static AuthSingleton Instance => _lazyInstance.Value;

        public bool IsAuthenticated()
        {
            return AuthToken != null;
        }
    }
}
