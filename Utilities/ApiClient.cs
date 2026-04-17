using Newtonsoft.Json;
using Petstore_Bdd_Tests_Csharp.Configurations;
using Petstore_Bdd_Tests_Csharp.Models;
using RestSharp;

namespace Petstore_Bdd_Tests_Csharp.Utilities
{
    public interface IApiClient
    {
        RestClient Client { get; }
        Task InitializeClientAsync();
        Task DisposeClientAsync();
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
    }
}
