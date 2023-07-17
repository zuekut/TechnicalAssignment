using CardanoAssignment.Models;

namespace CardanoAssignment.Extensions;

public static class HttpClientServiceExtensions
{
    public static void AddGleiHttpClient(this IServiceCollection serviceCollection, GleiApiConfiguration gleiApiConfiguration)
    {
        serviceCollection.AddHttpClient("Glei", httpClient => { httpClient.BaseAddress = new Uri(gleiApiConfiguration.BaseUrl); })
            .AddPolicyHandlerFromRegistry("DefaultWaitAndRetryStrategy")
            .AddPolicyHandlerFromRegistry("DefaultCircuitBreaker");
    }
}