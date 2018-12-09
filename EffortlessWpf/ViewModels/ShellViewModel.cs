using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Models;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EffortlessWpf.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        public BindableCollection<string> ServerSettings { get; set; }
        private string _selectedServerSetting = "No environment selected";
        private UserAccess _userAccess;

        private GenericDataViewModel<IEffortlessModel> CurrentViewModel { get; set; }

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
                    // load users
                }
                else
                {
                    Debug.WriteLine("WTTF");
                }

                NotifyOfPropertyChange(() => SelectedServerSetting);
            }
        }

        protected override void OnInitialize()
        {
            LoadUsersPage();
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

        public void LoadUsersPage()
        {
            //ActivateItem(new UsersViewModel());
            ActivateItem(new GenericDataViewModel<UserModel>("https://staging.effortless.dk/api"));
        }

        public void LoadCompaniesPage()
        {
            //ActivateItem(new CompaniesViewModel());
            //CurrentViewModel = new GenericDataViewModel<CompanyModel>("https://staging.effortless.dk/api");
            ActivateItem(new GenericDataViewModel<CompanyModel>("https://staging.effortless.dk/api"));
        }

        public void ActivateDataWindow()
        {
            ActivateItem(CurrentViewModel);
        }
    }
}
