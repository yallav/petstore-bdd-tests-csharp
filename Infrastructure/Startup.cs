using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petstore_Bdd_Tests_Csharp.Configurations;
using Petstore_Bdd_Tests_Csharp.EndPoints.Pet;
using Petstore_Bdd_Tests_Csharp.Utilities;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Petstore_Bdd_Tests_Csharp.Infrastructure
{
    public static class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            var environment = Environment.GetEnvironmentVariable("TEST_ENV") ?? "QA";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configurations/appsettings.json", optional: true)
                .AddJsonFile($"Configurations/appsettings.{environment}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var testSettings = configuration
                    .GetSection("TestSettings")
                    .Get<TestSettings>()
                    ?? throw new Exception("TestSettings configuration is missing");

            services.AddSingleton(testSettings);
            services.AddScoped<IApiClient, ApiClient>();
            services.AddScoped<IPostEndPoint, PostEndPoint>();
            services.AddScoped<IGetEndPoint, GetEndPoint>();
            services.AddScoped<IUpdateEndPoint, UpdateEndPoint>();
            services.AddScoped<IDeleteEndPoint, DeleteEndPoint>();
            services.AddScoped<IRequests, Requests>();

            return services;
        }
    }
}
