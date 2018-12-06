using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels
{
    class ShellViewModel : Screen
    {
        public async Task LoadUsers()
        {
            //Debug.WriteLine("Test123");
            var access = new UserAccess("https://api.effortless.dk/api");
            var users = await access.GetUsersAsync();
            Debug.WriteLine("Fetched all users.");

            //var access = new UserAccess("https://api.effortless.dk/api");

            //Debug.WriteLine("Loading all users");
            //var users = await access.TestGetUsersAsync();
            //Debug.WriteLine("All users were loaded.");
            Debug.WriteLine($"There are {users.Count} users in total.");

            foreach (var user in users)
            {
                Debug.WriteLine($"Loaded user: {user.UserName} : {user.FirstName} {user.LastName}");
            }
        }
    }
}
