using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels.Departments
{
    public class WorkPeriodControlViewModel : Screen
    {
        public WorkPeriodModel CurrentWorkPeriod { get; set; }
        public BindableCollection<AppointmentModel> Appointments { get; set; }
        public AppointmentModel SelectedAppointment { get; set; }
        public string _apiUrl { get; set; }
        public CompanyAccess _access { get; set; }

        public WorkPeriodControlViewModel(string apiUrl, WorkPeriodModel workPeriod)
        {
            CurrentWorkPeriod = workPeriod;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            Appointments = new BindableCollection<AppointmentModel>(CurrentWorkPeriod.Appointments);
            NotifyOfPropertyChange(() => Appointments);
        }

        public void MailSelectedUser()
        {
            string nl = "%0A";
            Process.Start($"mailto:{SelectedAppointment.Owner.Email}?subject={CurrentWorkPeriod.Department.Name} : " +
                $"Regarding your appointment at {SelectedAppointment.Start.ToShortDateString()}&" +
                $"body=Dear {SelectedAppointment.Owner.FullNameCapitalized},{nl}{nl}Sincerely,{nl}{CurrentWorkPeriod.Department.Name}{nl}");
        }
    }
}
