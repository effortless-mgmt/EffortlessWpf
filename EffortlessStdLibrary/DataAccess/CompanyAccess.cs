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

        public async Task<IEnumerable<DepartmentModel>> Departments(CompanyModel company)
        {
            return await _apiUrl
                .AppendPathSegments("Company", company.Id, "Departments")
                .GetJsonAsync<IEnumerable<DepartmentModel>>();
        }
    }
}
