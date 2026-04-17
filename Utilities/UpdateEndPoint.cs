using Newtonsoft.Json;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Utilities
{
    public interface IUpdateEndPoint
    {
        Task<T> PutAsync<T>(string endPoint, T pet) where T : class;
    }

    public class UpdateEndPoint(IApiClient apiClient) : IUpdateEndPoint
    {
        private readonly RestClient Client = apiClient.Client;

        public async Task<T> PutAsync<T>(string endPoint, T pet) where T : class
        {
            var request = new RestRequest(endPoint, Method.Put);
            request.AddJsonBody(pet);

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to update asset. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var updatedAsset = JsonConvert.DeserializeObject<T>(response.Content!);

            return updatedAsset ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }
    }
}
