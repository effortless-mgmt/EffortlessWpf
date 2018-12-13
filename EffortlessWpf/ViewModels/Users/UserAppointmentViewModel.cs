using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels.Users
{
    public class UserAppointmentViewModel : Screen
    {
        public BindableCollection<AppointmentModel> Appointments { get; set; }
        public AppointmentModel SelectedAppointment { get; set; }
        public UserModel CurrentUser { get; set; }
        public string WindowTitle { get; set; }
        private UserAccess _access;
        private string _apiUrl;

        public string ApiUrl
        {
            get => _apiUrl; 
            set
            {
                _apiUrl = value;
                _access.ApiUrl = ApiUrl;
            }
        }


        public UserAppointmentViewModel(string apiUrl, UserModel user)
        {
            _access = new UserAccess(apiUrl);
            ApiUrl = apiUrl;
            CurrentUser = user;
            WindowTitle = $"Appointments for {user.FullNameCapitalized}";
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await LoadAppointments();
        }

        private async Task LoadAppointments()
        {
            var workperiods = await _access.GetAppointmentsForUserAsync(CurrentUser);
            var appointments = new List<AppointmentModel>();

            foreach(var workperiod in workperiods)
            {
                foreach(var appointment in workperiod.Appointments)
                {
                    appointment.WorkPeriod = workperiod;
                    appointments.Add(appointment);
                }
            }

            Appointments = new BindableCollection<AppointmentModel>(appointments);
            NotifyOfPropertyChange(() => Appointments);
        }
    }
}
