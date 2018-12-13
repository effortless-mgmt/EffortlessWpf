using EffortlessStdLibrary.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessStdLibrary.DataAccess
{
    public class CompanyAccess
    {
        private readonly string _apiUrl;

        public CompanyAccess(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<IList<DepartmentModel>> Departments(CompanyModel company)
        {
            return await _apiUrl
                .AppendPathSegments("Company", company.Id, "Departments")
                .GetJsonAsync<IList<DepartmentModel>>();
        }

        public async Task<IList<WorkPeriodModel>> FetchWorkPeriodsAsync(DepartmentModel department)
        {
            return await _apiUrl
                .AppendPathSegments("Department", department.Id, "workperiod")
                .GetJsonAsync<IList<WorkPeriodModel>>();
        }

        //public async Task<IList<AppointmentModel>> FetchAppointments(WorkPeriodModel currentWorkPeriod)
        //{
        //    return await _apiUrl
        //        .AppendPathSegments()
        //        .GetJsonAsync<IList<AppointmentModel>>();
        //}
    }
}
