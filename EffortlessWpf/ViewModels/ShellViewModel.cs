using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessWpf.Models;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EffortlessWpf.ViewModels
{
    class ShellViewModel : Screen
    {
        public BindableCollection<UserModel> Users { get; set; }
        public BindableCollection<string> ServerSettings { get; set; }
        //public BindableCollection<string> ServerSettings { get; set; } = new BindableCollection<string>(new List<string> { "Production", "Staging" , "Local" });
        private string _selectedServerSetting = "No environment selected";
        private UserAccess _userAccess;

        public string SelectedServerSetting
        {
            get
            {
                return _selectedServerSetting;
            }
            set
            { 
                _selectedServerSetting = value;
                string serverUrl = null;
                Debug.WriteLine($"Selected {value} {_selectedServerSetting} {SelectedServerSetting}");
                
                if (SelectedServerSetting == "Production")
                {
                    serverUrl = "https://api.effortless.dk/api/";
                } 
                else if (SelectedServerSetting == "Staging")
                {
                    serverUrl = "https://staging.effortless.dk/api/";
                }
                else if (SelectedServerSetting == "Local")
                {
                    serverUrl = "http://10.20.0.98:5000/api/";
                }

                if (serverUrl != null)
                {
                    _userAccess = new UserAccess(serverUrl);
                   Task.Run(async () => await LoadUsers());
                }
                else
                {
                    Debug.WriteLine("WTTF");
                }

                NotifyOfPropertyChange(() => SelectedServerSetting);
            }
        }


        protected override async void OnInitialize()
        {
            SetServerSettings();
            //await LoadUsers();

        }

        private void SetServerSettings()
        {
            var sServerSettings = typeof(ServerSetting).GetFields();

            var settings = new List<string>(sServerSettings.Length);

            foreach (var s in sServerSettings)
            {
                if (!s.IsStatic) continue;
                settings.Add(s.Name);
            }

            ServerSettings = new BindableCollection<string>(settings);

            NotifyOfPropertyChange(() => ServerSettings);
        }

        public async Task LoadUsers()
        {
            Users = new BindableCollection<UserModel>(await _userAccess.GetUsersAsync());
            NotifyOfPropertyChange(() => Users);

            Debug.WriteLine($"There are {Users.Count} users in total.");

            foreach (var user in Users)
            {
                Debug.WriteLine($"Loaded user: {user.UserName} : {user.FirstName} {user.LastName}");
            }
        }

        public void SelectProduction()
        {
            SelectedServerSetting = "Production";
        }
        public void SelectStaging()
        {
            SelectedServerSetting = "Staging";
        }
        public void SelectLocal()
        {
            SelectedServerSetting = "Local";
        }
    }
}
