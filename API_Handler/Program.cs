using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Handler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup DI container
            var serviceProvider = ConfigureServices();

            // Get APIManager instance
            var apiManager = serviceProvider.GetRequiredService<APIManager>();

            // Example API request
            string response = await apiManager.SendRequest("GetFacts", HttpMethod.Get);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Load APIConfig once and register it in DI
            string configPath = "api_config.json";
            APIConfig apiConfig = APIConfig.LoadConfig(configPath);
            services.AddSingleton(apiConfig);

            // Register HttpClient & APIManager
            services.AddSingleton<HttpClient>();
            services.AddTransient<APIManager>();

            return services.BuildServiceProvider();
        }
    }
}
