using Caliburn.Micro;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Auth;
using EffortlessWpf.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        public BindableCollection<string> ServerSettings { get; set; }
        public string ServerUrl { get; private set; }
        public string AuthInfo { get; private set; }

        private dynamic _currentDataView;
        LoginControlViewModel LoginForm { get; set; } = new LoginControlViewModel(null);

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

                AuthSingleton.Instance.Logout();
                if (_currentDataView != null)
                {
                    UpdateCurrentViewData();
                }

                NotifyOfPropertyChange(() => SelectedServerSetting);
            }
        }

        public ShellViewModel()
        {
            AuthSingleton.Instance.OnLoginEvent += Login;
            AuthSingleton.Instance.OnLogoutEvent += Logout;
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
        public void OpenLoginPage()
        {
            LoginForm.ApiUrl = ServerUrl;
            LoginForm.OnSuccessfullLogin += Login;
            ActivateItem(LoginForm);
        }

        public void ActivateDataWindow<T>() where T : IEffortlessModel
        {
            if (!AuthSingleton.Instance.IsAuthenticated())
            {
                AuthSingleton.Instance.Logout();
                return;
            }

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

        public void Logout(object sender, TokenModel loginToken)
        {
            AuthInfo = "Not signed in";
            NotifyOfPropertyChange(() => AuthInfo);

            OpenLoginPage();
        }

        public void Login(object sender, TokenModel loginToken)
        {
            AuthInfo = $"Signed in as {loginToken.User.FullNameCapitalized}";
            NotifyOfPropertyChange(() => AuthInfo);

            DeactivateItem(LoginForm, true);
        }

        public async void Test(object o, EffortlessModelEventArgs args)
        {
            IWindowManager wm = new WindowManager();
            Debug.WriteLine($"I was double clicked. Id is {args.Model?.Id}");
            if (args.Model is UserModel user)
            {
                Debug.WriteLine($"You clicked on {user.FirstName} {user.LastName}.");
                var s = new Users.UserAppointmentViewModel(ServerUrl, user);
                s.OnDoubleClickEvent += UserDoubleClicked;
                wm.ShowWindow(s);
            }
            else if (args.Model is CompanyModel company)
            {
                //wm.ShowWindow(new CompanyDepartmentsViewModel(ServerUrl, company));
                wm.ShowWindow(new Departments.CompanySubViewModel(ServerUrl, company));
            }
        }

        public void UserDoubleClicked(object sender, DepartmentModel doubleClickedModel)
        {
            IWindowManager wm = new WindowManager();
            wm.ShowWindow(new Departments.CompanySubViewModel(ServerUrl, doubleClickedModel.Company));
        }
    }
}
