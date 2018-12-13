using Caliburn.Micro;
using EffortlessStdLibrary.DataAccess;
using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels
{
    public class CompanyDepartmentsViewModel : Screen
    {
        private CompanyAccess _companyAccess;
        public CompanyModel CurrentCompany { get; set; }
        public BindableCollection<DepartmentModel> Departments { get; set; }
        public BindableCollection<UserModel> WorkPeriods { get; set; }
        private DepartmentModel _departmentModel;
        public DepartmentModel SelectedDepartment
        {
            get
            {
                return _departmentModel;
            }
            set
            {
                _departmentModel = value;
            }
        }

        public string WindowTitle { get; set; }

        public string ApiUrl { get; set; }

        public CompanyDepartmentsViewModel(string apiUrl, CompanyModel company)
        {
            ApiUrl = apiUrl;
            CurrentCompany = company;
            _companyAccess = new CompanyAccess(ApiUrl);
            WindowTitle = company.Name;
        }

        protected override async void OnInitialize()
        {
            Debug.WriteLine("I loaded");
            await LoadDepartments();
            base.OnInitialize();

        }

        //public async Task LoadItemsAsync()
        //{
        //    Departments = new BindableCollection<DepartmentModel>(await _companyAccess.Departments(CurrentCompany));
        //    //Departments.CollectionChanged += HandleItemChange;

        //    NotifyOfPropertyChange(() => Departments);
        //}

        public async Task LoadWorkPeriods()
        {
            Debug.WriteLine("Loading departments...");
            //WorkPeriods = new BindableCollection<WorkPeriodModel>(await _companyAccess.FetchWorkPeriodsAsync(SelectedDepartment));
            var workPeriods = await _companyAccess.FetchWorkPeriodsAsync(SelectedDepartment);
            var users = workPeriods.SelectMany(wp => wp.AssignedUsers);
            WorkPeriods = new BindableCollection<UserModel>(users);
            Debug.WriteLine($"Loaded {WorkPeriods.Count()} departments.");
            NotifyOfPropertyChange(() => Departments);
            NotifyOfPropertyChange(() => WorkPeriods);
        }

        private async Task LoadDepartments()
        {
            var departments = await _companyAccess.Departments(CurrentCompany);
            //Departments = new BindableCollection<DepartmentModel>(departments);

            foreach (var department in departments)
            {
                department.WorkPeriods = await _companyAccess.FetchWorkPeriodsAsync(department);
            }

            Departments = new BindableCollection<DepartmentModel>(departments);

            NotifyOfPropertyChange(() => Departments);
        }
    }
}
