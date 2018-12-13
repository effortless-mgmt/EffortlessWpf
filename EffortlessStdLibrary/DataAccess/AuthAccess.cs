using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EffortlessStdLibrary.Models;
using Flurl;
using Flurl.Http;

namespace EffortlessStdLibrary.DataAccess
{
    public class AuthAccess
    {
        private readonly string _apiUrl;

        public AuthAccess(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<TokenModel> Authenticate(UserModel user)
        {
            return await _apiUrl
                .AppendPathSegments("auth", "login")
                .PostJsonAsync(user)
                .ReceiveJson<TokenModel>();
        }

        public async Task<TokenModel> Authenticate(string username, string password)
        {
            return await Authenticate(new UserModel { UserName = username, Password = password });
        }
    }
}
