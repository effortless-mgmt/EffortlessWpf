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
        public string ApiUrl { get; set; }

        public UserAccess(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        public async Task<ICollection<UserModel>> GetUsersAsync()
        {
            return await ApiUrl
                .AppendPathSegment("User")
                .GetJsonAsync<ICollection<UserModel>>();
        }

        public async Task DeleteUser(UserModel user)
        {
            await ApiUrl
                .AppendPathSegment("User")
                .AppendPathSegment(user.UserName)
                .DeleteAsync();
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            return await ApiUrl
                .AppendPathSegment("User")
                .PostJsonAsync(user)
                .ReceiveJson<UserModel>();
        }

        public async Task<IList<WorkPeriodModel>> GetAppointmentsForUserAsync(UserModel user)
        {
            return await ApiUrl
                .AppendPathSegments("User", user.UserName, "Appointment")
                .GetJsonAsync<IList<WorkPeriodModel>>();
        }
    }
}
