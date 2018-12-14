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

        public async Task DeleteUserAsync(UserModel user)
        {
            await ApiUrl
                .AppendPathSegment("User")
                .AppendPathSegment(user.UserName)
                .DeleteAsync();
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
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

        public async Task<IList<UserModel>> GetUsersByRoleAsync(RoleModel role)
        {
            return await ApiUrl
                .AppendPathSegment("User")
                .SetQueryParam("roleId", role.Id)
                .GetJsonAsync<IList<UserModel>>();
        }

        public async Task<IList<RoleModel>> GetRolesAsync()
        {
            return await ApiUrl
                .AppendPathSegments("Role")
                .GetJsonAsync<IList<RoleModel>>();
        }

        public async Task<RoleModel> GetRoleByRoleNameAsync(string roleName)
        {
            var role = await ApiUrl
                .AppendPathSegments("Role")
                .SetQueryParam("RoleName", roleName)
                .GetJsonAsync<IList<RoleModel>>();
            return role[0];
        }
    }
}
