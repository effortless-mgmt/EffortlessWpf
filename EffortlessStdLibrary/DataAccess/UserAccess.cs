using EffortlessStdLibrary.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessStdLibrary.DataAccess
{
    public class UserAccess
    {
        private readonly string _apiUrl;

        public UserAccess(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<ICollection<UserModel>> GetUsersAsync()
        {
            return await _apiUrl
                .AppendPathSegment("User")
                .GetJsonAsync<ICollection<UserModel>>();
        }

        public async Task DeleteUser(UserModel user)
        {
            await _apiUrl
                .AppendPathSegment("User")
                .AppendPathSegment(user.UserName)
                .DeleteAsync();
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            return await _apiUrl
                .AppendPathSegment("User")
                .PostJsonAsync(user)
                .ReceiveJson<UserModel>();
        }
    }
}
