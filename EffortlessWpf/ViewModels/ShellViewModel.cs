using Caliburn.Micro;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Models;
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
        public string ServerUrl { get; private set; }
        
        private dynamic _currentDataView;

        private ServerSetting _selectedServerSetting = ServerSetting.None;
        public ServerSetting SelectedServerSetting
        {
            get
            {
                return _selectedServerSetting;
            }
            set
            { 
                _selectedServerSetting = value;
                Debug.WriteLine($"Selected {value} {_selectedServerSetting} {SelectedServerSetting}");
                
                switch(value)
                {
                    case ServerSetting.Production:
                        ServerUrl = "https://api.effortless.dk/api/";
                        break;
                    case ServerSetting.Staging:
                        ServerUrl = "https://staging.effortless.dk/api/";
                        break;
                    case ServerSetting.Local:
                        ServerUrl = "http://10.20.0.98:5000/api/";
                        break;
                }

                if (_currentDataView != null)
                {
                    UpdateCurrentViewData();
                }

                NotifyOfPropertyChange(() => SelectedServerSetting);
            }
        }

        /// <summary>
        /// Fire and forget method to update the data in the current data view
        /// </summary>
        private void UpdateCurrentViewData()
        {
            List<UserModel> users = new List<UserModel>();
            users.Select(user => user.Email == "some@email.com").FirstOrDefault();
            _currentDataView.DataAccess.ApiUrl = ServerUrl;
            Task.Run(async () => await _currentDataView.LoadItemsAsync());
        }

        public void SelectProduction() => SelectedServerSetting = ServerSetting.Production;
        public void SelectStaging() => SelectedServerSetting = ServerSetting.Staging;
        public void SelectLocal() => SelectedServerSetting = ServerSetting.Local;

        public void LoadUsersPage() => ActivateDataWindow<UserModel>();
        public void LoadCompaniesPage() => ActivateDataWindow<CompanyModel>();
        public void LoadDepartmentsPage() => ActivateDataWindow<DepartmentModel>();

        public void ActivateDataWindow<T>() where T : IEffortlessModel
        {
            _currentDataView = new GenericDataViewModel<T>(ServerUrl);

            // Set server setting to production if nothing has been selected
            if (string.IsNullOrEmpty(ServerUrl))
            {
                SelectedServerSetting = ServerSetting.Production;
            }

            ActivateItem(_currentDataView);
        }
    }
}
