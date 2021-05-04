using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingModule.Web.Entities;
using ShoppingModule.Web.Services;
using System;
using System.Collections.Generic;

namespace ShoppingModule.Web.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IServicesContract _serviceContract;
        private readonly IConfiguration _configuration;
        public ProductService(IServicesContract serviceContract, IConfiguration configuration)
        {
            _serviceContract = serviceContract;
            _configuration = configuration;
        }

        public bool CreateProduct(Product product)
        {
            product.DateAdded = DateTime.Now;
            product.ExpiryDate = DateTime.Now.AddDays(10);
            string createProductUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:CreateProductUrl"];
            var response = _serviceContract.PostAsync<Response<bool>>(createProductUrl, JsonConvert.SerializeObject(product))?.Result;
            return response.Data;
        }

        public bool DeleteProduct(int id)
        {
            string productDeleteUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:ProductDeleteUrl"], id);
            var response = _serviceContract.GetAsync<Response<bool>>(productDeleteUrl)?.Result;
            return response.Data;
        }

        public IEnumerable<Product> GetAllProducts(string sortBy)
        {
            string productListUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:ProductListingUrl"], sortBy);
            var response = _serviceContract.GetAsync<Response<List<Product>>>(productListUrl)?.Result;
            return response?.Data;
        }

        public Product GetProductById(int? id)
        {
            string productByIdUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:GetProductById"], id);
            var response = _serviceContract.GetAsync<Response<Product>>(productByIdUrl)?.Result;
            return response?.Data;
        }

        public bool UpdateProduct(Product product)
        {
            string updateProductUrl = _configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:UpdateProductUrl"];
            var response = _serviceContract.PostAsync<Response<bool>>(updateProductUrl, JsonConvert.SerializeObject(product))?.Result;
            return response.Data;
        }

        public IEnumerable<Product> GetProductByCategory(string category, string sortBy)
        {
            string productByCategoryUrl = string.Format(_configuration["APIEndpoints:Domain"] + _configuration["APIEndpoints:ProductByCategoryUrl"], category, sortBy);
            var response = _serviceContract.GetAsync<Response<List<Product>>>(productByCategoryUrl)?.Result;
            return response?.Data;
        }
    }
}
