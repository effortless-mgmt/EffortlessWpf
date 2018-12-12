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
        public DepartmentModel SelectedDepartment { get; set; }
        public BindableCollection<DepartmentModel> Departments { get; set; }

        public string ApiUrl { get; set; }


        public CompanyDepartmentsViewModel(string apiUrl, CompanyModel company)
        {
            ApiUrl = apiUrl;
            CurrentCompany = company;
            _companyAccess = new CompanyAccess(ApiUrl);
        }

        protected override async void OnInitialize()
        {
            Debug.WriteLine("I loaded");
            await LoadDepartments();
            base.OnInitialize();

        }


        //protected override async void OnInitialize()
        //{
        //    await LoadDepartments();
        //}

        public async Task LoadItemsAsync()
        {
            Departments = new BindableCollection<DepartmentModel>(await _companyAccess.Departments(CurrentCompany));
            //Departments.CollectionChanged += HandleItemChange;

            NotifyOfPropertyChange(() => Departments);
        }






        private async Task LoadDepartments()
        {
            var departments = await _companyAccess.Departments(CurrentCompany);
            Departments = new BindableCollection<DepartmentModel>(departments);
            NotifyOfPropertyChange(() => Departments);
            Debug.WriteLine($"Loaded {departments.Count()} departments.");
        }
    }
}
