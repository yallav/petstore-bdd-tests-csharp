using Petstore_Bdd_Tests_Csharp.Utilities;

namespace Petstore_Bdd_Tests_Csharp.Support
{
    [Binding]
    public class Hooks(IApiClient apiClient)
    {
        [BeforeScenario]
        public async Task BeforeScenario()
        {
            await apiClient.InitializeClientAsync();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await apiClient.DisposeClientAsync();
        }
    }
}
