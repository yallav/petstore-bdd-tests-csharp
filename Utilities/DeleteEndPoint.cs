using Newtonsoft.Json;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Utilities
{
    public interface IDeleteEndPoint
    {
        Task<T> DeleteAsync<T>(string endPoint, long id) where T : class;
    }

    public class DeleteEndPoint(IApiClient apiClient) : IDeleteEndPoint
    {
        private readonly RestClient Client = apiClient.Client;

        public async Task<T> DeleteAsync<T>(string endPoint, long id) where T : class {
            var request = new RestRequest($"{endPoint}/{id}", Method.Delete);

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to delete asset. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var deletedAsset = JsonConvert.DeserializeObject<T>(response.Content!);

            return deletedAsset ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }
    }
}
