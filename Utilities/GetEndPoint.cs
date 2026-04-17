using Newtonsoft.Json;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Utilities
{
    public interface IGetEndPoint
    {
        Task<T> GetAsync<T>(string endPoint, long id) where T : class;
    }

    public class GetEndPoint(IApiClient apiClient) : IGetEndPoint
    {
        private readonly RestClient Client = apiClient.Client;

        public async Task<T> GetAsync<T>(string endPoint, long id) where T : class {
            var request = new RestRequest($"{endPoint}/{id}", Method.Get);

            if (Client == null)
            {
                throw new InvalidOperationException("RestClient is not initialized. Call InitializeClientAsync first.");
            }

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to retrieve asset. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var retrieveAsset = JsonConvert.DeserializeObject<T>(response.Content!);

            return retrieveAsset ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }
    }
}
