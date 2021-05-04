using System.Threading.Tasks;

namespace ShoppingModule.Web.Services
{
    public interface IServicesContract
    {
        /// <summary>
        /// Method will be called when need to make GET request to API
        /// </summary>
        /// <typeparam name="T">return data type</typeparam>
        /// <param name="url">api endpoint</param>
        /// <param name="token">bearer token</param>
        /// <returns>T Generic Object</returns>
        Task<T> GetAsync<T>(string url, string token = "");

        /// <summary>
        /// Method will be called when need to make POST request to API
        /// </summary>
        /// <typeparam name="T">return data type</typeparam>
        /// <param name="url">api endpoint</param>
        /// <param name="token">bearer token</param>
        /// <param name="json">request data</param>
        /// <returns>T Generic Object</returns>
        Task<T> PostAsync<T>(string url, string json, string token = "");

        /// <summary>
        /// Method will be called when need to make POST request to API
        /// </summary>
        /// <typeparam name="T">Return datatype</typeparam>        
        /// <param name="data">Input data object</param>
        /// <param name="url">URL of API that needs to be called</param>
        /// <returns></returns>
        Task<T> PostAsync<T, M>(M data, string url);
    }
}
