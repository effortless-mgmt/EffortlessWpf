using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using EffortlessWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels.Departments
{
    public class CompanyDepartmentControlViewModel : Screen
    {
        private CompanyAccess _access;
        public CompanyModel CurrentCompany { get; set; }
        public BindableCollection<DepartmentModel> Departments { get; set; }
        public DepartmentModel SelectedDepartment { get; set; }
        public event EventHandler<EffortlessModelEventArgs> OnDepartmentDoubleClick;

        public CompanyDepartmentControlViewModel(string apiUrl, CompanyModel currentCompany)
        {
            CurrentCompany = currentCompany;

            _access = new CompanyAccess(apiUrl);
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            await LoadDepartments();
        }

        private async Task LoadDepartments()
        {
            var departments = await _access.Departments(CurrentCompany);

            foreach(var department in departments)
            {
                department.WorkPeriods = await _access.FetchWorkPeriodsAsync(department);
            }

            Departments = new BindableCollection<DepartmentModel>(departments);
            NotifyOfPropertyChange(() => Departments);
        }

        public void DoubleClick() => OnDepartmentDoubleClick?.Invoke(this, new EffortlessModelEventArgs(SelectedDepartment));
    }
}
