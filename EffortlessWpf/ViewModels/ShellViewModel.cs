using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
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
            var dataView = new GenericDataViewModel<T>(ServerUrl);
            _currentDataView = dataView;

            // Set server setting to production if nothing has been selected
            if (string.IsNullOrEmpty(ServerUrl))
            {
                SelectedServerSetting = ServerSetting.Production;
            }

            dataView.DoubleClickedEventHandler += Test;
            ActivateItem(_currentDataView);
        }

        public async void Test(object o, EffortlessModelEventArgs args)
        {
            Debug.WriteLine($"I was double clicked. Id is {args.Model?.Id}");
            if (args.Model is UserModel user)
            {
                Debug.WriteLine($"You clicked on {user.FirstName} {user.LastName}.");
            }
            else if (args.Model is CompanyModel company)
            {
                IWindowManager wm = new WindowManager();
                //wm.ShowWindow(new CompanyDepartmentsViewModel(ServerUrl, company));
                wm.ShowWindow(new Departments.CompanySubViewModel(ServerUrl, company));
            }
        }
    }
}
