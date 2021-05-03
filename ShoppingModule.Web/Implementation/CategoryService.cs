using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingModule.Web.Interfaces;
using ShoppingModule.Web.Models;
using System.Collections.Generic;

namespace ShoppingModule.Web.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IServicesContract _serviceContract;
        private readonly IConfiguration _configuration;
        public CategoryService(IServicesContract serviceContract, IConfiguration configuration)
        {
            _serviceContract = serviceContract;
            _configuration = configuration;
        }

        public bool CreateCategory(Category category)
        {
            string createCategoryUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:CreateCategoryUrl"];
            var response = _serviceContract.PostAsync<Response<bool>>(createCategoryUrl, JsonConvert.SerializeObject(category))?.Result;
            return response.Data;
        }

        public bool DeleteCategory(int id)
        {
            string categoryDeleteUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:CategoryDeleteUrl"], id);
            var response = _serviceContract.GetAsync<Response<bool>>(categoryDeleteUrl)?.Result;
            return response.Data;
        }

        public IEnumerable<Category> GetAllCategories(string sortBy)
        {
            string categoryListUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:CategoryListingUrl"], sortBy);
            var response = _serviceContract.GetAsync<Response<List<Category>>>(categoryListUrl)?.Result;
            return response?.Data;
        }

        public Category GetCategoryById(int? id)
        {
            string categoryByIdUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:GetCategoryById"], id);
            var response = _serviceContract.GetAsync<Response<Category>>(categoryByIdUrl)?.Result;
            return response?.Data;
        }

        public bool UpdateCategory(Category category)
        {
            string updateCategoryUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:UpdateCategoryUrl"];
            var response = _serviceContract.PostAsync<Response<bool>>(updateCategoryUrl, JsonConvert.SerializeObject(category))?.Result;
            return response.Data;
        }
    }
}
