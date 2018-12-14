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
    public class UsersViewModel : Screen
    {
        public UserAccess _userAccess;
        //public GenericDataAccess<UserModel> _userAccess;
        public BindableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }

        public event EventHandler<UserModel> OnUserDoubleClick;

        private string _apiUrl;

        public string ApiUrl
        {
            get => _apiUrl;
            set
            {
                _apiUrl = value;
                _userAccess.ApiUrl = _apiUrl;

            }
        }

        private bool _onlyShowSubstitutes;

        public bool OnlyShowSubstitutes
        {
            get => _onlyShowSubstitutes;
            set
            {
                _onlyShowSubstitutes = value;
                if (value)
                {
                    Task.Run(async () => await LoadSubstitutesAsync());
                }
                else
                {
                    Task.Run(async () => await LoadAllUsersAsync());
                }
            }
        }

        public UsersViewModel(string apiUrl)
        {
            _userAccess = new UserAccess(apiUrl);
            ApiUrl = apiUrl;
        }

        protected override async void OnInitialize()
        {
            Debug.WriteLine("Initializing users view");
            await LoadAllUsersAsync();
        }

        public async Task LoadAllUsersAsync()
        {
            Users = new BindableCollection<UserModel>(await _userAccess.GetUsersAsync());
            Users.CollectionChanged += HandleUserChangeAsync;

            NotifyOfPropertyChange(() => Users);
        }

        public async Task LoadSubstitutesAsync()
        {
            var substituteRole = await _userAccess.GetRoleByRoleNameAsync("substitute");
            var users = await _userAccess.GetUsersByRoleAsync(substituteRole);

            Users = new BindableCollection<UserModel>(users);
            Users.CollectionChanged += HandleUserChangeAsync;

            NotifyOfPropertyChange(() => Users);
        }

        private async void HandleUserChangeAsync(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var s in e.OldItems)
                {
                    var user = (UserModel)s;
                    await _userAccess.DeleteUserAsync(user);
                    await LoadAllUsersAsync();
                }
            }
        }

        public void DoubleClick() => OnUserDoubleClick?.Invoke(this, SelectedUser);
    }
}
