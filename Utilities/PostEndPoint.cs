using Newtonsoft.Json;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Utilities
{
    public interface IPostEndPoint
    {
        Task<T> CreateAsync<T>(string endPoint, T pet) where T : class;
    }

    public class PostEndPoint(IApiClient apiClient) : IPostEndPoint
    {
        private readonly RestClient Client = apiClient.Client;

        public async Task<T> CreateAsync<T>(string endPoint, T pet) where T : class
        {
            if (Client == null)
            {
                throw new InvalidOperationException(
                    "RestClient is not initialized. Call InitializeClientAsync first.");
            }

            var request = new RestRequest(endPoint, Method.Post);
            request.AddJsonBody(pet);

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to create asset. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var createdAsset = JsonConvert.DeserializeObject<T>(response.Content!);

            return createdAsset ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }
    }
}
