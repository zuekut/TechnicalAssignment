using CardanoAssignment.Models;

namespace CardanoAssignment.Extensions;

public static class HttpClientServiceExtensions
{
    public static void AddGleiHttpClient(this IServiceCollection serviceCollection, GleiApiConfiguration gleiApiConfiguration)
    {
        serviceCollection.AddHttpClient("Glei", httpClient => { httpClient.BaseAddress = new Uri(gleiApiConfiguration.BaseUrl); })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Added since there was an issue with executing requests in docker-compose file. To fix it, the most likely solution would be to create a self-signed certificate for the docker container and run the container via https
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
                }

                return handler;
            })
            .AddPolicyHandlerFromRegistry("DefaultWaitAndRetryStrategy")
            .AddPolicyHandlerFromRegistry("DefaultCircuitBreaker");
    }
}