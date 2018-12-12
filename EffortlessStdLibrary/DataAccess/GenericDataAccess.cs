using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EffortlessStdLibrary.DataAccess
{
    public class GenericDataAccess<T> where T : IEffortlessModel
    {
        private string _apiUrl;

        public string ApiUrl
        {
            get { return _apiUrl; }
            set { _apiUrl = value; }
        }

        // 5 corresponds to the length of "Model", so UserModel outputs "User" and "CompanyModel" outputs "Company"
        private string _modelName => typeof(T).Name.Remove(typeof(T).Name.Length - 5, 5);
        
        public GenericDataAccess(string apiUrl) => _apiUrl = apiUrl;

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _apiUrl
                .AppendPathSegment(_modelName)
                .GetJsonAsync<ICollection<T>>();
        }

        public async Task<T> GetSpecificAsync(T model)
        {
            if (model is UserModel)
            {
                return await _apiUrl
                    .AppendPathSegments(_modelName, ((UserModel)(object)model).UserName)
                    .GetJsonAsync<T>();
            }
            else
            {
                return await _apiUrl
                    .AppendPathSegments(_modelName, model.Id)
                    .GetJsonAsync<T>();
            }
        }

        public async Task<T> CreateAsync(T model)
        {
            return await _apiUrl
                .AppendPathSegment(_modelName)
                .PostJsonAsync(model)
                .ReceiveJson<T>();
        }

        public async Task DeleteAsync(T model)
        {
            if (model is UserModel)
            {
                await _apiUrl
                    .AppendPathSegments(_modelName, ((UserModel)(object)model).UserName)
                    .DeleteAsync();
            } 
            else
            {
                await _apiUrl
                    .AppendPathSegments(_modelName, model.Id)
                    .DeleteAsync();
            }
        }
    }
}
