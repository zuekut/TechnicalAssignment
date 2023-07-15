namespace CardanoAssignment.Extensions;

public static class HttpClientServiceExtensions
{
    public static void AddGleiHttpClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient("Glei", httpClient => { httpClient.BaseAddress = new Uri("https://api.gleif.org/"); });
    }
}