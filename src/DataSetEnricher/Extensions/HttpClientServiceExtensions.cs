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
                //ToDo Figure out how to trust the root certificate inside the Docker image
                //Added a workaround (Only for Development) until I figure out how to fix it since there was an issue with executing requests in Docker container, does not happen locally. There is an issue with the client SSL certificate towards api.gleif.org, resulting in Untrusted root.
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