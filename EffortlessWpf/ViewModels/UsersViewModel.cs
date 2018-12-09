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
        //public UserAccess _userAccess;
        public GenericDataAccess<UserModel> _userAccess;
        public BindableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }


        public UsersViewModel()
        {
            _userAccess = new GenericDataAccess<UserModel>("https://staging.effortless.dk/api");
        }

        protected override async void OnInitialize()
        {
            Debug.WriteLine("Initializing users view");
            await LoadUsers();
        }

        public async Task LoadUsers()
        {
            Users = new BindableCollection<UserModel>(await _userAccess.GetAllAsync());
            Users.CollectionChanged += HandleUserChange;

            NotifyOfPropertyChange(() => Users);
        }

        private async void HandleUserChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var s in e.OldItems)
                {
                    var user = (UserModel)s;
                    await _userAccess.DeleteAsync(user);
                    await LoadUsers();
                }
            }
        }
    }
}
