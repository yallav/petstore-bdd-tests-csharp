using Newtonsoft.Json;
using Petstore_Bdd_Tests_Csharp.Configurations;
using Petstore_Bdd_Tests_Csharp.Models;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Support
{
    public interface IApiClient
    {
        Task InitializeClientAsync();
        Task DisposeClientAsync();
        Task<PetModel> CreatePetAsync(PetModel pet);
        Task<PetModel> GetPetAsync(long id);
        Task<PetModel> UpdatePetAsync(PetModel pet);
        Task<ApiErrorResponse> DeletePetAsync(long id);
    }

    public class ApiClient(TestSettings testSettings) : IApiClient
    {
        private RestClient? _client;

        public RestClient Client => _client!;

        public async Task InitializeClientAsync()
        {
            _client = new RestClient(testSettings.BaseUri);
        }

        public async Task DisposeClientAsync()
        {
            _client?.Dispose();
        }

        public async Task<PetModel> CreatePetAsync(PetModel pet)
        {
            if (Client == null)
            {
                throw new InvalidOperationException(
                    "RestClient is not initialized. Call InitializeClientAsync first.");
            }

            var request = new RestRequest("/pet", Method.Post);
            request.AddJsonBody(pet);

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to create pet. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var createdPet = JsonConvert.DeserializeObject<PetModel>(response.Content!);

            return createdPet ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }

        public async Task<PetModel> GetPetAsync(long id)
        {
            var request = new RestRequest($"/pet/{id}", Method.Get);

            if (Client == null)
            {
                throw new InvalidOperationException("RestClient is not initialized. Call InitializeClientAsync first.");
            }

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to retrieve pet. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var existingPet = JsonConvert.DeserializeObject<PetModel>(response.Content!);

            return existingPet ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }

        public async Task<PetModel> UpdatePetAsync(PetModel pet)
        {
            var request = new RestRequest("/pet", Method.Put);
            request.AddJsonBody(pet);

            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to update pet. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var updatedPet = JsonConvert.DeserializeObject<PetModel>(response.Content!);

            return updatedPet ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }

        public async Task<ApiErrorResponse> DeletePetAsync(long id)
        {
            var request = new RestRequest($"/pet/{id}", Method.Delete);
            var response = await Client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Failed to delete pet. StatusCode: {response.StatusCode}, Response: {response.Content}");
            }

            var updatedPet = JsonConvert.DeserializeObject<ApiErrorResponse>(response.Content!);

            return updatedPet ?? throw new Exception("Deserialization failed. Response content was null or invalid.");
        }
    }
}
