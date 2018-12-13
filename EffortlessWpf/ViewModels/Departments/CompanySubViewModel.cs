using Caliburn.Micro;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels.Departments
{
    public class CompanySubViewModel : Conductor<object>
    {
        public CompanyModel CurrentCompany { get; set; }
        public string _apiUrl;
        public string WindowTitle { get; set; }

        public CompanySubViewModel(string apiUrl, CompanyModel currentCompany)
        {
            _apiUrl = apiUrl;
            CurrentCompany = currentCompany;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            OpenDepartments();
        }

        private void OpenDepartments()
        {
            var departmentControl = new CompanyDepartmentControlViewModel(_apiUrl, CurrentCompany);
            departmentControl.OnDepartmentDoubleClick += OnDepartmentDoubleClick;

            ActivateItem(departmentControl);
        }

        private void OpenWorkPeriod(WorkPeriodModel wp)
        {
            var workPeriodControl = new WorkPeriodControlViewModel(_apiUrl, wp);

            ActivateItem(workPeriodControl);
        }

        private void OnDepartmentDoubleClick(object o, EffortlessModelEventArgs args)
        {
            Debug.WriteLine("Double CLick");
            if (args.Model is WorkPeriodModel wp)
            {
                Debug.WriteLine($"Clicked on work period {wp.Name}.");
                OpenWorkPeriod(wp);
            }
            else if (args.Model is DepartmentModel d)
            {
                Debug.WriteLine($"Selected department {d.Name}");
                if (d.WorkPeriods.Count() == 1)
                {
                    Debug.WriteLine($"{d.Name} only have a single work period, {d.WorkPeriods[0].Name}, opening that.");
                    OpenWorkPeriod(d.WorkPeriods[0]);
                }
            }
        }
    }
}
